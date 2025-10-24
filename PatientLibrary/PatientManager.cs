using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PatientLibrary
{
    public class PatientManager
    {
        private readonly string filePath = Path.Combine("Data", "data.json");
        private List<PatientDetails> patients;

        public PatientManager()
        {
            LoadData();
        }

        private void LoadData() 
        {
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory("Data");
                File.WriteAllText(filePath, "[]");
            }

            var json = File.ReadAllText(filePath);
            patients = JsonConvert.DeserializeObject<List<PatientDetails>>(json) ?? new List<PatientDetails>();
        }

        private void SaveData()
        {
            var json = JsonConvert.SerializeObject(patients, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void AddPatient(PatientDetails patient)
        {
            patient.Id = patients.Any() ? patients.Max(p => p.Id) + 1 : 1;
            patients.Add(patient);
            SaveData();
        }

        public bool RemovePatient(long id)
        {
            var patient = patients.FirstOrDefault(p => p.Id == id);
            if (patient != null)
            {
                patients.Remove(patient);
                SaveData();
                return true;
            }
            return false;
        }

        public bool UpdatePatient(long id, Action<PatientDetails> updateAction)
        {
            var patient = patients.FirstOrDefault(p => p.Id == id);
            if (patient != null)
            {
                updateAction(patient);
                SaveData();
                return true;
            }
            return false;
        }

        public List<PatientDetails> GetAllPatients() => patients;
        public List<PatientDetails> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<PatientDetails>();

            return patients
                .Where(p => !string.IsNullOrWhiteSpace(p.Name) &&
                            p.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }
        public List<PatientDetails> SearchByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return new List<PatientDetails>();

            return patients
                .Where(p => !string.IsNullOrWhiteSpace(p.Email) &&
                            p.Email.ToLower().Contains(email.ToLower()))
                .ToList();
        }
        public List<PatientDetails> SearchByMobile(long mobile)
        {
            return patients.Where(p => p.Mobile == mobile).ToList();
        }
        public List<PatientDetails> SearchByLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return new List<PatientDetails>();

            return patients
                .Where(p => !string.IsNullOrWhiteSpace(p.Location) &&
                            p.Location.ToLower().Contains(location.ToLower()))
                .ToList();
        }


    }
}
