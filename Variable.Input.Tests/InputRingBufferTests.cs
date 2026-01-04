using Xunit;

namespace Variable.Input.Tests;

public class InputRingBufferTests
{
    [Fact]
    public void Enqueue_SingleInput_Works()
    {
        var buffer = new InputRingBuffer();
        var result = buffer.RegisterInput(1);

        Assert.True(result);
        Assert.Equal(1, buffer.Count);
    }

    [Fact]
    public void Enqueue_MaxCapacity_Works()
    {
        var buffer = new InputRingBuffer();
        for (var i = 0; i < InputRingBuffer.CAPACITY; i++) Assert.True(buffer.RegisterInput(1));

        Assert.Equal(InputRingBuffer.CAPACITY, buffer.Count);
    }

    [Fact]
    public void Enqueue_OverCapacity_Fails()
    {
        var buffer = new InputRingBuffer();
        for (var i = 0; i < InputRingBuffer.CAPACITY; i++) buffer.RegisterInput(1);

        var result = buffer.RegisterInput(2);

        Assert.False(result);
        Assert.Equal(InputRingBuffer.CAPACITY, buffer.Count);
    }

    [Fact]
    public void Dequeue_EmptyBuffer_Fails()
    {
        var buffer = new InputRingBuffer();
        var result = buffer.TryDequeue(out var input);

        Assert.False(result);
        Assert.Equal(InputId.None, input);
    }

    [Fact]
    public void Dequeue_FIFO_Order()
    {
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);
        buffer.RegisterInput(2);
        buffer.RegisterInput(1);

        Assert.True(buffer.TryDequeue(out var input1));
        Assert.Equal(1, input1);

        Assert.True(buffer.TryDequeue(out var input2));
        Assert.Equal(2, input2);

        Assert.True(buffer.TryDequeue(out var input3));
        Assert.Equal(1, input3);

        Assert.Equal(0, buffer.Count);
    }

    [Fact]
    public void Peek_DoesNotConsume()
    {
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);

        Assert.True(buffer.Peek(out var input1));
        Assert.Equal(1, input1);
        Assert.Equal(1, buffer.Count);

        Assert.True(buffer.Peek(out var input2));
        Assert.Equal(1, input2);
        Assert.Equal(1, buffer.Count);
    }

    [Fact]
    public void RingBuffer_WrapAround_Works()
    {
        var buffer = new InputRingBuffer();

        for (var i = 0; i < InputRingBuffer.CAPACITY; i++) buffer.RegisterInput(1);

        for (var i = 0; i < 4; i++) buffer.TryDequeue(out _);

        Assert.Equal(4, buffer.Count);

        for (var i = 0; i < 4; i++) Assert.True(buffer.RegisterInput(2));

        Assert.Equal(8, buffer.Count);

        for (var i = 0; i < 4; i++)
        {
            buffer.TryDequeue(out var input);
            Assert.Equal(1, input);
        }

        for (var i = 0; i < 4; i++)
        {
            buffer.TryDequeue(out var input);
            Assert.Equal(2, input);
        }
    }

    [Fact]
    public void Extension_RegisterInput_Works()
    {
        var buffer = new InputRingBuffer();
        Assert.True(buffer.RegisterInput(1));
        Assert.Equal(1, buffer.Count);
    }

    [Fact]
    public void Extension_Clear_Works()
    {
        var buffer = new InputRingBuffer();
        buffer.RegisterInput(1);
        buffer.RegisterInput(2);

        buffer.Clear();

        Assert.Equal(0, buffer.Count);
        Assert.Equal(0, buffer.Head);
        Assert.Equal(0, buffer.Tail);
    }

    [Fact]
    public void Complex_Interleaved_ReadWrite_Works()
    {
        var buffer = new InputRingBuffer();
        
        // 1. Fill halfway
        for (int i = 1; i <= 4; i++) Assert.True(buffer.RegisterInput(i));
        Assert.Equal(4, buffer.Count);
        
        // 2. Read 2
        Assert.True(buffer.TryDequeue(out int val)); Assert.Equal(1, val);
        Assert.True(buffer.TryDequeue(out val)); Assert.Equal(2, val);
        Assert.Equal(2, buffer.Count);
        
        // 3. Fill to wrap around (Capacity 8. Head=2, Tail=4. Add 6 items -> Tail should wrap)
        // Current items: [_, _, 3, 4, _, _, _, _]
        // Add 5,6,7,8,9,10
        for (int i = 5; i <= 10; i++) Assert.True(buffer.RegisterInput(i));
        
        Assert.Equal(8, buffer.Count); // Full
        
        // 4. Verify Full
        Assert.False(buffer.RegisterInput(11)); // Should fail
        
        // 5. Read all and verify order
        int[] expected = { 3, 4, 5, 6, 7, 8, 9, 10 };
        foreach (var exp in expected)
        {
            Assert.True(buffer.TryDequeue(out val));
            Assert.Equal(exp, val);
        }
        
        Assert.Equal(0, buffer.Count);
    }
}