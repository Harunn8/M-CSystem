using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Snmp;
using Infrastructure.Tcp;
using Infrastructure.Data;


namespace Application.Services
{
    public class CommunicationService
    {
        private readonly SnmpCommunication _snmpCommunication;

        public CommunicationService(SnmpCommunication snmpCommunication)
        {
                _snmpCommunication = snmpCommunication;
        }

        public void StartCommunication()
        {
            Console.WriteLine("Starting SNMP Communication...");
            _snmpCommunication.StartSnmpPolling("10.0.20.69");
        }

        public List<string> GetDeviceData()
        {
            return _snmpCommunication.GetAllData();
        }

        public void SetValue(string ipAddress, string oid, string value)
        {
            Console.WriteLine("Setting value...");
            _snmpCommunication.SetParamValue(ipAddress, oid, value);
        }

        }
    }
