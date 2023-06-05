
namespace Chipsoft.Data.Repositories
{       
    // Repository classes exchange data between the data context and the business logic layer of the application.
    // These Repository classes hide the logics required to store and retrieve data.
    // The repository pattern of design separates the logic that stores and retrieves data from the business logic that acts on the data.
    public class PhysicianRepository : IPhysicianRepository
    {        
        private readonly EPDDbContext _context;
                
        public PhysicianRepository()
        {
            _context = new EPDDbContext();
        }
                
        public PhysicianRepository(EPDDbContext context)
        {
            _context = context;
        }

        //This method will return all the Physicians from the Physician table
        public IEnumerable<Physician> GetAll()
        {
            return _context.Physicians.ToList();
        }
              
        public Physician? GetById(int id)
        {
            return _context.Physicians.Find(id);
        }
              
        public void Insert(Physician physician)
        {
            // There are tracking methods on DbSet and on DbContext.
            // Here we track via DbSet.
            _context.Physicians.Add(physician);
            // Tracking via DbContext would be _context.Add(physician) and then the context will discover the type.
        }
                

        public void DeleteById(int id)
        {            
            Physician? physician = _context.Physicians.Find(id);
                        
            if (physician != null)
            {                
                _context.Physicians.Remove(physician);
            }

        }

        //This method will make the changes permanent in the database
        //That means once we call Insert, Update, and Delete Methods, then we need to call
        //the Save method to make the changes permanent in the database
        public void Save()
        {
            //Based on the Entity State, it will generate the corresponding SQL Statement and
            //Execute the SQL Statement in the database
            //For Added Entity State: It will generate INSERT SQL Statement
            //For Modified Entity State: It will generate UPDATE SQL Statement
            //For Deleted Entity State: It will generate DELETE SQL Statement
            _context.SaveChanges();
        }
        private bool disposed = false;

        //As a context object is a heavy object or you can say time-consuming object
        //So, once the operations are done we need to dispose of the same using Dispose method
        //The EmployeeDBContext class inherited from DbContext class and the DbContext class
        //is Inherited from the IDisposable interface

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}