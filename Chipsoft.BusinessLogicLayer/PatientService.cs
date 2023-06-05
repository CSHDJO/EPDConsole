using Chipsoft.Data;
using Chipsoft.Data.Repositories;

namespace Chipsoft.BusinessLogicLayer
{
    public class PatientService
    {
        private AddressRepository _addressRepository;
        private AppointmentRepository _appointmentRepository;
        private PatientRepository _patientRepository;
        private PhysicianRepository _physicianRepository;

        public PatientService()
        {
            EPDDbContext dbContext = new EPDDbContext();
            MakeRepositories(dbContext);
        }

        public PatientService(EPDDbContext dbContext)
        {
            MakeRepositories(dbContext);
        }

        private void MakeRepositories(EPDDbContext dbContext)
        {
            _addressRepository = new AddressRepository(dbContext);
            _appointmentRepository = new AppointmentRepository(dbContext);
            _patientRepository = new PatientRepository(dbContext);
            _physicianRepository = new PhysicianRepository(dbContext);
        }

        private Address AddAddress(string land, string stad, string straat, string huisnummer)
        {
            // Object Initializer Syntax
            Address address = new Address()
            {
                Country = land,
                City = stad,
                Street = straat,
                HouseNumber = huisnummer
            };
            _addressRepository.Insert(address);
            _addressRepository.Save();
            return address;
        }

        public void AddPatient(string naam, string contactgegeven, string land, string stad, string straat, string huisnummer)
        {
            Address address = AddAddress(land, stad, straat, huisnummer);

            Patient patient = new Patient();
            patient.Name = naam;
            patient.Contactgegeven = contactgegeven;
            patient.AddressID = address.AddressID;
            _patientRepository.Insert(patient);
            _patientRepository.Save();
        }

        public void DeletePatient(string naam)
        {
            // Here I give an example of using Linq.
            var patients = _patientRepository.GetAll().Where(p => p.Name == naam);

            foreach (var p in patients)
            {
                _patientRepository.DeleteById(p.PatientID);
                _addressRepository.DeleteById(p.AddressID);
                _patientRepository.Save();
                _addressRepository.Save();
            }
        }

        public List<AppointmentItem> ShowAppointmentForPatient(string naam)
        {
            List<AppointmentItem> list = new List<AppointmentItem>();
            var allAppointments = _appointmentRepository.GetAll();

            foreach (var appointment in allAppointments)
            {
                Physician? physician = _physicianRepository.GetById(appointment.PhysicianID);
                Patient? patient = _patientRepository.GetById(appointment.PatientID);

                if (physician != null && patient != null && patient.Name == naam)
                {
                    AppointmentItem item = new AppointmentItem();
                    item.Description = appointment.Description;
                    item.PatientName = patient.Name;
                    item.PhysicianName = physician.Name;
                    list.Add(item);
                }
            }
            return list;
        }



    }
}