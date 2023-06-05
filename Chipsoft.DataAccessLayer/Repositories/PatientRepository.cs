
namespace Chipsoft.Data.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly EPDDbContext _context;

        public PatientRepository()
        {
            _context = new EPDDbContext();
        }

        public PatientRepository(EPDDbContext context)
        {
            _context = context;
        }

        //This method will return all the Patients from the Patient table
        public IEnumerable<Patient> GetAll()
        {
            return _context.Patients.ToList();
        }

        public Patient? GetById(int id)
        {
            return _context.Patients.Find(id);
        }

        public void Insert(Patient patient)
        {
            _context.Patients.Add(patient);
        }


        public void DeleteById(int id)
        {
            Patient? patient = _context.Patients.Find(id);

            if (patient != null)
            {
                _context.Patients.Remove(patient);
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