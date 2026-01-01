namespace Variable.Input.Tests;

public class InputRingBufferTests
{
    [Fact]
    public void Enqueue_SingleInput_Works()
    {
        var buffer = new InputRingBuffer();
        var result = ComboLogic.TryEnqueueInput(ref buffer, 1);

        Assert.True(result);
        Assert.Equal(1, buffer.Count);
    }

    [Fact]
    public void Enqueue_MaxCapacity_Works()
    {
        var buffer = new InputRingBuffer();
        for (var i = 0; i < InputRingBuffer.CAPACITY; i++) Assert.True(ComboLogic.TryEnqueueInput(ref buffer, 1));

        Assert.Equal(InputRingBuffer.CAPACITY, buffer.Count);
    }

    [Fact]
    public void Enqueue_OverCapacity_Fails()
    {
        var buffer = new InputRingBuffer();
        for (var i = 0; i < InputRingBuffer.CAPACITY; i++) ComboLogic.TryEnqueueInput(ref buffer, 1);

        var result = ComboLogic.TryEnqueueInput(ref buffer, 2);

        Assert.False(result);
        Assert.Equal(InputRingBuffer.CAPACITY, buffer.Count);
    }

    [Fact]
    public void Dequeue_EmptyBuffer_Fails()
    {
        var buffer = new InputRingBuffer();
        var result = ComboLogic.TryDequeueInput(ref buffer, out var input);

        Assert.False(result);
        Assert.Equal(InputId.None, input);
    }

    [Fact]
    public void Dequeue_FIFO_Order()
    {
        var buffer = new InputRingBuffer();
        ComboLogic.TryEnqueueInput(ref buffer, 1);
        ComboLogic.TryEnqueueInput(ref buffer, 2);
        ComboLogic.TryEnqueueInput(ref buffer, 1);

        Assert.True(ComboLogic.TryDequeueInput(ref buffer, out var input1));
        Assert.Equal(1, input1);

        Assert.True(ComboLogic.TryDequeueInput(ref buffer, out var input2));
        Assert.Equal(2, input2);

        Assert.True(ComboLogic.TryDequeueInput(ref buffer, out var input3));
        Assert.Equal(1, input3);

        Assert.Equal(0, buffer.Count);
    }

    [Fact]
    public void Peek_DoesNotConsume()
    {
        var buffer = new InputRingBuffer();
        ComboLogic.TryEnqueueInput(ref buffer, 1);

        Assert.True(ComboLogic.PeekInput(ref buffer, out var input1));
        Assert.Equal(1, input1);
        Assert.Equal(1, buffer.Count);

        Assert.True(ComboLogic.PeekInput(ref buffer, out var input2));
        Assert.Equal(1, input2);
        Assert.Equal(1, buffer.Count);
    }

    [Fact]
    public void RingBuffer_WrapAround_Works()
    {
        var buffer = new InputRingBuffer();

        for (var i = 0; i < InputRingBuffer.CAPACITY; i++) ComboLogic.TryEnqueueInput(ref buffer, 1);

        for (var i = 0; i < 4; i++) ComboLogic.TryDequeueInput(ref buffer, out _);

        Assert.Equal(4, buffer.Count);

        for (var i = 0; i < 4; i++) Assert.True(ComboLogic.TryEnqueueInput(ref buffer, 2));

        Assert.Equal(8, buffer.Count);

        for (var i = 0; i < 4; i++)
        {
            ComboLogic.TryDequeueInput(ref buffer, out var input);
            Assert.Equal(1, input);
        }

        for (var i = 0; i < 4; i++)
        {
            ComboLogic.TryDequeueInput(ref buffer, out var input);
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
}