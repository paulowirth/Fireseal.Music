using Abstra.Challenge.Tests.Unit.Fakes;

namespace Abstra.Challenge.Tests.Unit.Infrastructure.Persistence;

public sealed class DbContextRepositoryTests(InMemoryFixture inMemoryFixture) : IClassFixture<InMemoryFixture>
{
    [Fact]
    public async Task Exists_ShouldReturnTrue_WhenEntityExists()
    {
        //Arrange
        var fakeEntity = 
            new FakeEntity
            {
                Id  = Guid.CreateVersion7(), 
                Name = nameof(FakeEntity) 
            };
        
        await using var fakeContext = new FakeContext(inMemoryFixture.CreateDbContextOptions());
        var fakeRepository = new FakeRepository(fakeContext);
        await fakeRepository.Insert(fakeEntity, CancellationToken.None);
        
        //Act
        var exists = 
            await fakeRepository.Exists(entity => entity.Id == fakeEntity.Id, CancellationToken.None);
        
        //Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task Get_ShouldInsert_ReturnsExpected()
    {
        //Arrange
        var fakeEntity = 
            new FakeEntity
            {
                Id = Guid.CreateVersion7(), 
                Name = nameof(FakeEntity) 
            };
        
        await using var fakeContext = new FakeContext(inMemoryFixture.CreateDbContextOptions());
        var fakeRepository = new FakeRepository(fakeContext);
        await fakeRepository.Insert(fakeEntity, CancellationToken.None);

        //Act
        var result = await fakeRepository.Get(fakeEntity.Id, [], CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEntity.Id, result.Id);
        Assert.Equal(fakeEntity.Name, result.Name);
    }

    [Fact]
    public async Task Get_ShouldIncludeNavigationProperties_ReturnsExpected()
    {
        //Arrange
        var fakeEntity = 
            new FakeEntity
            {
                Id = Guid.CreateVersion7(), 
                Name = nameof(FakeEntity),
                Children = [new FakeEntity { Id = Guid.CreateVersion7(), Name = nameof(FakeEntity) }]
            };
        
        await using var fakeContext = new FakeContext(inMemoryFixture.CreateDbContextOptions());
        var fakeRepository = new FakeRepository(fakeContext);
        await fakeRepository.Insert(fakeEntity, CancellationToken.None);

        //Act
        var result = 
            await fakeRepository.Get(fakeEntity.Id, [entity => entity.Children], CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Children);
    }

    [Fact]
    public async Task Get_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        //Arrange
        await using var fakeContext = new FakeContext(inMemoryFixture.CreateDbContextOptions());
        var fakeRepository = new FakeRepository(fakeContext);

        //Act
        var result = await fakeRepository.Get(Guid.NewGuid(), [], CancellationToken.None);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task List_ShouldReturnAllEntities()
    {
        //Arrange
        var fakeEntity1 = new FakeEntity { Id = Guid.CreateVersion7(), Name = nameof(FakeEntity) };
        var fakeEntity2 = new FakeEntity { Id = Guid.CreateVersion7(), Name = nameof(FakeEntity) };
        
        await using var fakeContext = new FakeContext(inMemoryFixture.CreateDbContextOptions());
        var fakeRepository = new FakeRepository(fakeContext);
        
        await fakeRepository.Insert(fakeEntity1, CancellationToken.None);
        await fakeRepository.Insert(fakeEntity2, CancellationToken.None);

        //Act
        var result = await fakeRepository.List([], CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, entity => entity.Id == fakeEntity1.Id);
        Assert.Contains(result, entity => entity.Id == fakeEntity2.Id);
    }

    [Fact]
    public async Task List_ShouldIncludeNavigationProperties_ReturnsExpected()
    {
        //Arrange
        var fakeEntity1 = 
            new FakeEntity
            {
                Id = Guid.CreateVersion7(), 
                Name = nameof(FakeEntity),
                Children = [new FakeEntity { Id = Guid.CreateVersion7(), Name = nameof(FakeEntity) }]
            };
        
        var fakeEntity2 = 
            new FakeEntity
            {
                Id = Guid.CreateVersion7(), 
                Name = nameof(FakeEntity),
                Children = [new FakeEntity { Id = Guid.CreateVersion7(), Name = nameof(FakeEntity) }]
            };
        
        await using var fakeContext = new FakeContext(inMemoryFixture.CreateDbContextOptions());
        var fakeRepository = new FakeRepository(fakeContext);
        await fakeRepository.Insert(fakeEntity1, CancellationToken.None);
        await fakeRepository.Insert(fakeEntity2, CancellationToken.None);

        //Act
        var result = 
            await fakeRepository.List([fake => fake.Children], CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Single(fake => fake.Id == fakeEntity1.Id).Children);
        Assert.NotEmpty(result.Single(fake => fake.Id == fakeEntity2.Id).Children);
    }

    [Fact]
    public async Task Insert_ShouldAdd_ReturnsExpected()
    {
        //Arrange
        var fakeEntity = 
            new FakeEntity
            {
                Id = Guid.CreateVersion7(), 
                Name = nameof(FakeEntity)
            };
        
        await using var fakeContext = new FakeContext(inMemoryFixture.CreateDbContextOptions());
        var fakeRepository = new FakeRepository(fakeContext);

        //Act
        await fakeRepository.Insert(fakeEntity, CancellationToken.None);
        var result = await fakeRepository.Get(fakeEntity.Id, [], CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEntity.Name, result.Name);
    }

    [Fact]
    public async Task Update_ShouldSaveChanges_ReturnsExpected()
    {
        //Arrange
        var fakeEntity = 
            new FakeEntity
            {
                Id = Guid.CreateVersion7(), 
                Name = nameof(FakeEntity) 
            };
        
        const string expectedName = "AfterUpdate";
        var dbContextOptions = inMemoryFixture.CreateDbContextOptions();
        
        await using (var insertContext = new FakeContext(dbContextOptions))
        {
            var fakeRepository = new FakeRepository(insertContext);
            await fakeRepository.Insert(fakeEntity, CancellationToken.None);
        }
        
        fakeEntity.Name = expectedName;
        
        await using (var updateContext = new FakeContext(dbContextOptions))
        {
            //Act
            var fakeRepository = new FakeRepository(updateContext);
            var updated = await fakeRepository.Update(fakeEntity, [], CancellationToken.None);
            var result = await fakeRepository.Get(fakeEntity.Id, [], CancellationToken.None);
            
            //Assert
            Assert.True(updated);
            Assert.NotNull(result);
            Assert.Equal(expectedName, result.Name);
        }
    }

    [Fact]
    public async Task Update_ShouldNotSaveChanges_WhenEntityDoesNotExist()
    {
        //Arrange
        var fakeEntity =
            new FakeEntity
            {
                Id = Guid.CreateVersion7(), 
                Name = nameof(FakeEntity) 
            };
        
        await using var fakeContext = new FakeContext(inMemoryFixture.CreateDbContextOptions());
        var fakeRepository = new FakeRepository(fakeContext);

        //Act
        var updated = await fakeRepository.Update(fakeEntity, [], CancellationToken.None);
        
        //Assert
        Assert.False(updated);
    }

    [Fact]
    public async Task Delete_ShouldSaveChanges_ReturnsExpected()
    {
        //Arrange
        var fakeEntity = 
            new FakeEntity
            {
                Id = Guid.CreateVersion7(), 
                Name = nameof(FakeEntity) 
            };
        
        var dbContextOptions = inMemoryFixture.CreateDbContextOptions();
        
        await using (var insertContext = new FakeContext(dbContextOptions))
        {
            var fakeRepository = new FakeRepository(insertContext);
            await fakeRepository.Insert(fakeEntity, CancellationToken.None);
        }

        //Act
        await using (var updateContext = new FakeContext(dbContextOptions))
        {
            var fakeRepository = new FakeRepository(updateContext);
            var deleted = await fakeRepository.Delete(fakeEntity.Id, CancellationToken.None);
            var getResult = await fakeRepository.Get(fakeEntity.Id, [], CancellationToken.None);

            // Assert
            Assert.True(deleted);
            Assert.Null(getResult);
        }
    }

    [Fact]
    public async Task Delete_ShouldReturnFalse_WhenEntityDoesNotExist()
    {
        //Arrange
        await using var fakeContext = new FakeContext(inMemoryFixture.CreateDbContextOptions());
        var fakeRepository = new FakeRepository(fakeContext);

        //Act
        var deleted = await fakeRepository.Delete(Guid.NewGuid(), CancellationToken.None);

        //Assert
        Assert.False(deleted);
    }
}
