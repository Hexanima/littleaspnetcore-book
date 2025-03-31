using Little_ASP.NET_Core_Book.Data;
using Little_ASP.NET_Core_Book.Models;
using Little_ASP.NET_Core_Book.Services;
using Microsoft.EntityFrameworkCore;

namespace Little_ASP.NET_Core_Book.UnitTests;

public class TodoItemServiceShould
{
    [Fact]
    public async Task AddNewItemAsIncompleteWithDueDate()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;

        await using (ApplicationDbContext context = new(options))
        {
            SQLiteTodoItemService service = new(context);

            AppUser fakeUser = new()
            {
                Id = Guid.NewGuid(),
                UserName = "usuario@falso.com"
            };

            await service.SaveItem(new NewTodoItem
            {
                Title = "Testing?",
                DueAt = DateTimeOffset.Now.AddDays(3)
            }, fakeUser);
        }

        await using (ApplicationDbContext context = new(options))
        {
            int itemsInDatabase = await context.Items.CountAsync();

            Assert.Equal(1, itemsInDatabase);

            TodoItem item = await context.Items.FirstAsync();

            Assert.Equal("Testing?", item.Title);
            Assert.False(item.IsDone);

            TimeSpan? difference = DateTimeOffset.Now.AddDays(3) - item.DueAt;

            Assert.True(difference < TimeSpan.FromSeconds(1));
        }
    }

    [Fact]
    public async Task MarkDoneFailsWhenNonexistentItemId()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_FailingMarkDoneWrongId").Options;

        await using ApplicationDbContext context = new(options);
        SQLiteTodoItemService service = new SQLiteTodoItemService(context);
        AppUser fakeUser = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = "usuario@falso.com"
        };

        await service.SaveItem(new NewTodoItem
        {
            Title = "Testing?",
            DueAt = DateTimeOffset.Now.AddDays(3)
        }, fakeUser);
        bool result = await service.MarkDone(Guid.NewGuid(), fakeUser);

        Assert.False(result);

        TodoItem item = await context.Items.FirstAsync();
        Assert.False(item.IsDone);
    }

    [Fact]
    public async Task MarkDoneFailsWhenIncorrectUser()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_FailingMarkDoneWrongUser").Options;

        await using ApplicationDbContext context = new(options);

        SQLiteTodoItemService service = new(context);
        AppUser fakeUser = new()
        {
            Id = Guid.NewGuid(),
            UserName = "usuario@falso.com"
        };
        AppUser anotherUser = new()
        {
            Id = Guid.NewGuid(),
            UserName = "usuario@falso.com"
        };

        await service.SaveItem(new NewTodoItem
        {
            Title = "Testing?",
            DueAt = DateTimeOffset.Now.AddDays(3)
        }, fakeUser);

        TodoItem item = await context.Items.FirstAsync();
        bool result = await service.MarkDone(item.Id, anotherUser);

        Assert.False(result);

        TodoItem itemResult = await context.Items.FirstAsync();
        Assert.False(itemResult.IsDone);
    }

    [Fact]
    public async Task MarkDoneSucceedsWhenValidData()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_SuccessfulMarkDone").Options;

        await using ApplicationDbContext context = new(options);

        SQLiteTodoItemService service = new(context);
        AppUser fakeUser = new()
        {
            Id = Guid.NewGuid(),
            UserName = "usuario@falso.com"
        };

        await service.SaveItem(new NewTodoItem
        {
            Title = "Testing?",
            DueAt = DateTimeOffset.Now.AddDays(3)
        }, fakeUser);

        TodoItem item = await context.Items.FirstAsync();
        bool result = await service.MarkDone(item.Id, fakeUser);

        Assert.True(result);

        TodoItem itemResult = await context.Items.FirstAsync();
        Assert.True(itemResult.IsDone);
    }

    [Fact]
    public async Task GetIncompleteItemsReturnsOnlyAUserItems()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_GetUserSpecificIncompleteItems").Options;

        AppUser userOne = new()
        {
            Id = Guid.NewGuid(),
            UserName = "user@one.com"
        };
        await using (ApplicationDbContext context = new(options))
        {
            SQLiteTodoItemService service = new(context);

            AppUser userTwo = new()
            {
                Id = Guid.NewGuid(),
                UserName = "user@one.com"
            };
            AppUser userThree = new()
            {
                Id = Guid.NewGuid(),
                UserName = "user@one.com"
            };

            await service.SaveItem(new NewTodoItem
            {
                Title = "Uno"
            }, userOne);
            await service.SaveItem(new NewTodoItem
            {
                Title = "Uno"
            }, userTwo);
            await service.SaveItem(new NewTodoItem
            {
                Title = "Uno"
            }, userThree);
            await service.SaveItem(new NewTodoItem
            {
                Title = "Dos"
            }, userThree);
            await service.SaveItem(new NewTodoItem
            {
                Title = "Dos"
            }, userOne);
            await service.SaveItem(new NewTodoItem
            {
                Title = "Tres"
            }, userOne);
            await service.SaveItem(new NewTodoItem
            {
                Title = "Cuatro"
            }, userOne);
            TodoItem aDoneItem = await context.Items.Where(x => x.Title == "Cuatro").FirstAsync();
            await service.MarkDone(aDoneItem.Id, userOne);
        }

        await using (ApplicationDbContext context = new(options))
        {
            SQLiteTodoItemService service = new(context);

            TodoItem[] result = await service.GetIncompleteItemsAsync(userOne);

            Assert.All(result, x => Assert.False(x.IsDone));
            Assert.All(result, x => Assert.Equal(userOne.Id, x.UserId));
            Assert.Equal(3, result.Length);
        }
    }
}