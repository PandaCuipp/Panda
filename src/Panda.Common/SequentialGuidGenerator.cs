using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Panda.Common
{
    public class SequentialGuidGenerator
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        /// <summary>
        /// Gets the singleton SequentialGuidGenerator instance.
        /// </summary>
        public static SequentialGuidGenerator Instance { get; } = new SequentialGuidGenerator();

        public SequentialGuidGenerator.SequentialGuidDatabaseType DatabaseType { get; set; }

        /// <summary>
        /// Prevents a default instance of the SequentialGuidGenerator class from being created.
        /// Use SequentialGuidGenerator.Instance
        /// </summary>
        private SequentialGuidGenerator()
        {
            this.DatabaseType = SequentialGuidGenerator.SequentialGuidDatabaseType.SqlServer;
        }

        public Guid Create()
        {
            return this.Create(this.DatabaseType);
        }

        public Guid Create(SequentialGuidGenerator.SequentialGuidDatabaseType databaseType)
        {
            switch (databaseType)
            {
                case SequentialGuidGenerator.SequentialGuidDatabaseType.SqlServer:
                    return this.Create(SequentialGuidGenerator.SequentialGuidType.SequentialAtEnd);
                case SequentialGuidGenerator.SequentialGuidDatabaseType.Oracle:
                    return this.Create(SequentialGuidGenerator.SequentialGuidType.SequentialAsBinary);
                case SequentialGuidGenerator.SequentialGuidDatabaseType.MySql:
                    return this.Create(SequentialGuidGenerator.SequentialGuidType.SequentialAsString);
                case SequentialGuidGenerator.SequentialGuidDatabaseType.PostgreSql:
                    return this.Create(SequentialGuidGenerator.SequentialGuidType.SequentialAsString);
                default:
                    throw new InvalidOperationException();
            }
        }

        public Guid Create(SequentialGuidGenerator.SequentialGuidType guidType)
        {
            byte[] randomBytes = new byte[10];
            lock (Rng)
            {
                Rng.GetBytes(randomBytes);
            }
            //SequentialGuidGenerator.Rng.Locking<RandomNumberGenerator>((Action<RandomNumberGenerator>)(r => r.GetBytes(randomBytes)));

            byte[] bytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks / 10000L);
            if (BitConverter.IsLittleEndian)
                Array.Reverse((Array)bytes);
            byte[] b = new byte[16];
            switch (guidType)
            {
                case SequentialGuidGenerator.SequentialGuidType.SequentialAsString:
                case SequentialGuidGenerator.SequentialGuidType.SequentialAsBinary:
                    Buffer.BlockCopy((Array)bytes, 2, (Array)b, 0, 6);
                    Buffer.BlockCopy((Array)randomBytes, 0, (Array)b, 6, 10);
                    if (guidType == SequentialGuidGenerator.SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse((Array)b, 0, 4);
                        Array.Reverse((Array)b, 4, 2);
                        break;
                    }
                    break;
                case SequentialGuidGenerator.SequentialGuidType.SequentialAtEnd:
                    Buffer.BlockCopy((Array)randomBytes, 0, (Array)b, 0, 10);
                    Buffer.BlockCopy((Array)bytes, 2, (Array)b, 10, 6);
                    break;
            }
            return new Guid(b);
        }

        /// <summary>Database type to generate GUIDs.</summary>
        public enum SequentialGuidDatabaseType
        {
            SqlServer,
            Oracle,
            MySql,
            PostgreSql,
        }

        /// <summary>Describes the type of a sequential GUID value.</summary>
        public enum SequentialGuidType
        {
            /// <summary>
            /// The GUID should be sequential when formatted using the
            /// <see cref="M:System.Guid.ToString" /> method.
            /// </summary>
            SequentialAsString,
            /// <summary>
            /// The GUID should be sequential when formatted using the
            /// <see cref="M:System.Guid.ToByteArray" /> method.
            /// </summary>
            SequentialAsBinary,
            /// <summary>
            /// The sequential portion of the GUID should be located at the end
            /// of the Data4 block.
            /// </summary>
            SequentialAtEnd,
        }
    }
}
