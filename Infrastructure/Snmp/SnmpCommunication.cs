using System;
using System.Collections.Generic;
using SnmpSharpNet;
using System.Net;

namespace Infrastructure.Snmp
{
    public class SnmpCommunication
    {
        private readonly List<string> oids = new List<string>
        {
            ".1.3.6.1.4.1.18837.3.3.2.4.0",
            ".1.3.6.1.4.1.18837.3.3.2.5.0",
            ".1.3.6.1.4.1.18837.3.3.2.6.0",
            ".1.3.6.1.4.1.18837.3.3.2.7.0",
            ".1.3.6.1.4.1.18837.3.3.2.2.0",
            ".1.3.6.1.4.1.18837.3.3.2.1.0",
            ".1.3.6.1.4.1.18837.3.2.2.2.1.1.13.1",
            ".1.3.6.1.4.1.18837.3.2.2.2.1.1.14.1",
            ".1.3.6.1.4.1.18837.3.2.2.2.1.1.15.1",
            ".1.3.6.1.4.1.18837.3.2.2.2.1.1.6.1",
            ".1.3.6.1.4.1.18837.3.2.2.2.1.1.8.1",
            ".1.3.6.1.4.1.18837.3.2.2.2.1.1.16.1",
            ".1.3.6.1.4.1.18837.3.2.2.2.1.1.17.1",
            ".1.3.6.1.4.1.18837.3.2.2.1.8.0",
            ".1.3.6.1.4.1.18837.3.2.2.1.9.0",
            ".1.3.6.1.4.1.18837.3.2.2.1.6.0",
            ".1.3.6.1.4.1.18837.3.2.2.1.5.0",
            ".1.3.6.1.4.1.18837.3.2.2.1.7.0",
            ".1.3.6.1.4.1.18837.3.2.2.1.15.0",
            ".1.3.6.1.4.1.18837.3.2.2.1.1.0",
            ".1.3.6.1.4.1.18837.3.2.2.1.1.0",
            ".1.3.6.1.4.1.18837.3.2.2.1.2.0",
            ".1.3.6.1.4.1.18837.3.2.2.1.13.0",
            ".1.3.6.1.4.1.18837.3.2.2.1.14.0",
            ".1.3.6.1.4.1.18837.3.2.2.5.1.0"
        };

        // List in all oid
        private readonly List<string> _allData = new List<string>();

        // Public method to start polling without community name
        public void StartSnmpPolling(string ipAddress, int port = 5007)
        {
            try
            {
                // SNMP agent parameters, community name removed
                AgentParameters param = new AgentParameters()
                {
                    Version = SnmpVersion.Ver1 // Use SnmpVersion.Ver2 if needed
                };

                // Target SNMP agent IP and port
                IpAddress agentAddress = new IpAddress(ipAddress);
                UdpTarget target = new UdpTarget((IPAddress)agentAddress, port, 2000, 100); // Timeout 2000ms, retry 2 times

                // Perform polling
                SnmpPoll(target, param);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StartSnmpPolling Exception: {ex.Message}");
            }
        }

        // Private method to poll SNMP data
        private void SnmpPoll(UdpTarget target, AgentParameters param)
        {
            foreach (var oid in oids)
            {
                Pdu pdu = new Pdu(PduType.Get);
                pdu.VbList.Add(oid);

                try
                {
                    // Send SNMP request
                    SnmpV1Packet response = (SnmpV1Packet)target.Request(pdu, param);

                    if (response != null)
                    {
                        if (response.Pdu.ErrorStatus == 0) // No errors
                        {
                            var result = $"OID: {oid} - Value: {response.Pdu.VbList[0].Value}";
                            _allData.Add(result); // Değeri listeye ekle
                            Console.WriteLine(result); // Print response value
                        }
                        else
                        {
                            // Print error code and message
                            Console.WriteLine($"SNMP Error for OID {oid}: ErrorStatus={response.Pdu.ErrorStatus}, ErrorIndex={response.Pdu.ErrorIndex}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"SNMP response is null for OID {oid}");
                    }
                }
                catch (SnmpNetworkException ex)
                {
                    // Handle SNMP-specific exceptions
                    Console.WriteLine($"Network error for OID {oid}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // General exceptions
                    Console.WriteLine($"General error for OID {oid}: {ex.Message}");
                }
            }
        }

           public void SetParamValue(string ipAddress,string oid, string value, int port = 5001)
           {
              try
              {
                AgentParameters param = new AgentParameters
                {
                    Version = SnmpVersion.Ver1
                };

                IpAddress agentAddress = new IpAddress(ipAddress);
                UdpTarget target = new UdpTarget((IPAddress)agentAddress, port, 2000, 2);

                Pdu pdu = new Pdu(PduType.Set);
                pdu.VbList.Add(new Oid(oid),new OctetString(value));

                SnmpV1Packet response = (SnmpV1Packet)target.Request(pdu, param);

                if (response != null)
                {
                    if (response.Pdu.ErrorStatus == 0) // No errors
                    {
                        Console.WriteLine($"Successfully set OID {oid} to value {value}");
                    }
                    else
                    {
                        Console.WriteLine($"SNMP Error for OID {oid}: ErrorStatus={response.Pdu.ErrorStatus}, ErrorIndex={response.Pdu.ErrorIndex}");
                    }
                }
                else
                {
                    Console.WriteLine($"SNMP response is null for OID {oid}");
                }
            }
            catch(SnmpNetworkException ex) 
              {
                Console.WriteLine($" Error: {ex.Message}");  
              }
           }

        // Public method to get all received SNMP data
        public List<string> GetAllData()
        {
            return _allData;
        }
    }
}
