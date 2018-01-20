using System;
using System.Collections.Generic;
using System.Threading;

namespace DataAccessProtection
{

    #region Interfaces

    public interface IBuffer
    {
        byte[] Buffer { get; }
    }

    public interface IBufferRegistration : IBuffer, IDisposable {}

    #endregion

    #region Large Object Heap Buffer

    public class LOHBuffer : IBuffer
    {
        private readonly byte[] _buffer;
        internal bool InUse { get; set; }
        public byte[] Buffer { get { return _buffer; } }

        public LOHBuffer()
        {
            _buffer = new byte[LOHBufferMin];
        }

        private const int LOHBufferMin = 85000;
    }

    #endregion

    #region BufferPool Implementation

    public class BufferPool
    {
        private SemaphoreSlim _guard;
        // Holds all LOHBuffers of the BufferPool
        // and also serves as lock object.
        private List<LOHBuffer> _buffers;

        public BufferPool(int maxSize)
        {
            // The SemaphoreSlim manages number of accesses on its own
            // using maxSize passed in during construction. Incrementing
            // and decrementing CurrentCount of threads having acquired
            // the lock is automatically taken care of.
            _guard = new SemaphoreSlim(maxSize);
            _buffers = new List<LOHBuffer>(maxSize);
        }

        public IBufferRegistration GetBuffer()
        {
            // this blocks until a buffer is free
            _guard.Wait();

            // can now get buffer so make sure we're the only thread manipulating
            // the list of buffers
            lock (_buffers)
            {
                IBufferRegistration freeBuffer = null;

                // look for a free buffer
                foreach (LOHBuffer buffer in _buffers)
                {
                    if (!buffer.InUse)
                    {
                        buffer.InUse = true;
                        freeBuffer = new BufferReservation(this, buffer);
                    }
                }

                // no free buffer so allocate a new one
                if (freeBuffer == null)
                {
                    LOHBuffer buffer = new LOHBuffer { InUse = true };
                    _buffers.Add(buffer);
                    freeBuffer = new BufferReservation(this, buffer);
                }

                return freeBuffer;
            }
        }

        private void Release(LOHBuffer buffer)
        {
            // flag buffer as no longer in use and release the semaphore
            // to allow more requests into the pool
            buffer.InUse = false;
            _guard.Release();
        }

        // Nested class
        class BufferReservation : IBufferRegistration
        {
            private readonly BufferPool pool;
            private readonly LOHBuffer buffer;

            public BufferReservation(BufferPool pool, LOHBuffer buffer)
            {
                this.pool = pool;
                this.buffer = buffer;
            }

            public byte[] Buffer
            {
                get { return buffer.Buffer; }
            }

            public void Dispose()
            {
                pool.Release(buffer);
            }
        }
    }

    #endregion

}