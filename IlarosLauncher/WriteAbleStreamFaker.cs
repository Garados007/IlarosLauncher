using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IlarosLauncher
{
    public class WriteAbleStreamFaker : Stream
    {
        Stream stream;
        bool discardChanges, disposeBaseStream;
        long virtualLength;
        long position;

        public WriteAbleStreamFaker(Stream baseStream, bool discardChanges, bool disposeBaseStream)
        {
            stream = baseStream ?? throw new ArgumentNullException("baseStream");
            this.discardChanges = discardChanges;
            if (!discardChanges) throw new NotImplementedException("caching changes is not implemented now");
            this.disposeBaseStream = disposeBaseStream;

            virtualLength = stream.Length;
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public override long Length => stream.Length;

        public override long Position
        {
            get => position;
            set
            {
                if (value < 0 || value >= virtualLength) throw new ArgumentOutOfRangeException();
                position = value;
                if (position < stream.Length) stream.Position = value;
            }
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var t = 0;
            int l;
            if (position < stream.Length && position < virtualLength)
            {
                l = (int)Math.Min(count, Math.Min(stream.Length - position, virtualLength - position));
                var r = stream.Read(buffer, offset, l);
                count -= r;
                offset += r;
                t += r;
                position += r;
            }
            l = (int)Math.Min(count, virtualLength - position);
            for (int i = 0; i < l; ++i) buffer[i + offset] = 0;
            position += l;
            return t + l;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin: return Position = offset;
                case SeekOrigin.End: return Position = virtualLength + offset - 1;
                default: return Position = position + offset;
            }
        }

        public override void SetLength(long value)
        {
            virtualLength = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            position += count;
            if (virtualLength < position) virtualLength = position;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposeBaseStream) stream.Dispose();
        }
    }
}
