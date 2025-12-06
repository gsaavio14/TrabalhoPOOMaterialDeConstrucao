using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Interface
{
    internal interface IDao<T>
    {
        void Create(T t);


        void Update(T t);

        void Delete(int id);


        List<T> GetAll();
    }
}
