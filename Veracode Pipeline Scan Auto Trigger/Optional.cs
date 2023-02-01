using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veracode_Pipeline_Scan_Auto_Trigger.globals
{
    public class Optional<T>
    {
        public bool IsPresent { get; private set; }

        private T value;

        public T Get()
        {
            if (IsPresent)
            {
                return value;
            }
            throw new InvalidOperationException("Value is not present");
        }

        private Optional(T value)
        {
            this.value = value;
            IsPresent = true;
        }


        private Optional()
        {
            this.value = default(T);
            IsPresent = false;
        }

        public static Optional<T> Of(T value)
        {
            if (value == null)
            {
                throw new InvalidOperationException("Value can't be null");
            }
            return new Optional<T>(value);
        }

        public static Optional<T> OfNullable(T value)
        {
            if (value == null)
            {
                return Empty();
            }
            else
            {
                return Of(value);
            }
        }

        private static Optional<T> Empty()
        {
            return new Optional<T>();
        }

        public override bool Equals(object other)
        {
            if (other is Optional<T>)
            {
                return this.Equals((Optional<T>)other);
            }
            else
            {
                return false;
            }
        }
        public bool Equals(Optional<T> other)
        {
            if (IsPresent && other.IsPresent)
            {
                return object.Equals(value, other.value);
            }
            else
            {
                return IsPresent == other.IsPresent;
            }
        }

        public void IfPresent(Action<T> toRun)
        {
            if (this.IsPresent)
            {
                toRun.Invoke(this.value);
            }
        }

        public Optional<K> Map<K>(Func<T, K> toRun)
        {
            if (IsPresent)
            {
                return Optional<K>.OfNullable(toRun.Invoke(value));
            }
            return Optional<K>.Empty();
        }

        public T OrElse(Func<T> methodToRun)
        {
            return IsPresent ? value : methodToRun.Invoke();
        }

        public T OrElse(T alternativeValue)
        {
            return IsPresent ? value : alternativeValue;
        }
    }
}
