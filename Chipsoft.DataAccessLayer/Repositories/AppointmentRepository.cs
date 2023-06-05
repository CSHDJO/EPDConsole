
namespace Chipsoft.Data.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly EPDDbContext _context;

        public AppointmentRepository()
        {
            _context = new EPDDbContext();
        }

        public AppointmentRepository(EPDDbContext context)
        {
            _context = context;
        }

        //This method will return all the Appointments from the Appointment table
        public IEnumerable<Appointment> GetAll()
        {
            return _context.Appointments.ToList();
        }

        public Appointment? GetById(int id)
        {
            return _context.Appointments.Find(id);
        }

        public void Insert(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
        }


        public void DeleteById(int id)
        {
            Appointment? appointment = _context.Appointments.Find(id);

            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
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