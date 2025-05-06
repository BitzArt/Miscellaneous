namespace BitzArt.Wolverine.Extensions.Tests;

public class TopicBuilderTests
{
    [Fact]
    public void Build_WithTwoTopicsPerTwoQueues_ReturnsCorrectMapping()
    {
        // Arrange
        var topicBuilder = new TopicBuilder("test");
        
        topicBuilder.Topic("Topic-1")
            .ToQueue("Queue1")
            .ToQueue("Queue2");
        
        topicBuilder.Topic("Topic-2")
            .ToQueue("Queue1")
            .ToQueue("Queue2");

        // Act
        var mapping = topicBuilder.Build();


        // Assert
        Assert.Equal(2, mapping.Count);
        
        Assert.Equal(2, mapping["Topic-1"].Count);
        Assert.Equal(2, mapping["Topic-2"].Count);
        
        Assert.Equal("Queue1", mapping["Topic-1"][0]);
        Assert.Equal("Queue2", mapping["Topic-1"][1]);
        
        Assert.Equal("Queue1", mapping["Topic-1"][0]);
        Assert.Equal("Queue2", mapping["Topic-2"][1]);
    }
    
    [Fact]
    public void Build_WithNestedTopicsAndQueues_ReturnsCorrectMapping()
    {
        // Arrange
        var topicBuilder = new TopicBuilder("test");
        
        topicBuilder
            .Topic("Topic-1")
                .ToQueue("Queue1")
                .ToQueue("Queue2")
            .Topic("Topic-2")
                .ToQueue("Queue3");

        // Act
        var mapping = topicBuilder.Build();


        // Assert
        Assert.Equal(2, mapping.Count);
        
        Assert.Equal(2, mapping["Topic-1"].Count);
        Assert.Equal(1, mapping["Topic-2"].Count);
        
        Assert.Equal("Queue3", mapping["Topic-2"][0]);
    }
    
    [Fact]
    public void Build_WithMultipleTopicsWithoutQueues_ReturnsEmptyMapping()
    {
        // Arrange
        var topicBuilder = new TopicBuilder("test");

        topicBuilder
            .Topic("Topic-1")
            .Topic("Topic-2");

        // Act
        var mapping = topicBuilder.Build();


        // Assert
        Assert.Empty(mapping);
    }

}