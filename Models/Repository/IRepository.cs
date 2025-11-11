using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository;

    public interface IRepository<T> where T : class
    {
        // READ ALL - Henter alle elementer fra databasen
        IEnumerable<T> GetAll();

        // READ BY ID - Finder et specifikt element
        T GetById(int id);

        // CREATE - Opretter en ny post i databasen
        int Add(T entity);

        // UPDATE - Opdaterer en eksisterende post
        void Update(T entity);

        // DELETE - Sletter en post
        void Delete(int id);
    }

