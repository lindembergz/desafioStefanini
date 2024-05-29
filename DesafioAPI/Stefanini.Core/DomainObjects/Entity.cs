using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stefanini.Core.DomainObjects
{
    public abstract class Entity<T>
    {
        public T Id { get; set; }
        protected Entity()
        {
            //Id = 0;
        }

        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}
