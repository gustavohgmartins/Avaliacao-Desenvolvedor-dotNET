using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AvaliacaoDotNetCsharp
{
    class Program
    {
        
        private static string getUsersInput(){
            try{
                Console.WriteLine("\nSelect one of the following options:");
                Console.WriteLine("1- Search contact");
                Console.WriteLine("2- Add contact");
                Console.WriteLine("3- Edit contact");
                Console.WriteLine("4- List contact numbers");
                Console.WriteLine("5- Add contact number");
                Console.WriteLine("6- Remove contact");
                Console.WriteLine("7- Remove contact number");
                Console.WriteLine("C- Clear Screen");
                Console.WriteLine("X- Exit\n");

                string userInput = Console.ReadLine().ToUpper();
                return userInput;
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                return null;
            }
        }
        static void Main(string[] args)
        {
            try{
                string[] options = {"1","2","3","4","5","6","7","C","X"};
                string userInput = getUsersInput();
                while(userInput != "X"){
                    if(options.Contains(userInput)){
                        switch(userInput){
                            case "1":
                                getContact();
                                break;
                            case "2":
                                addContact();
                                break;
                            case "3":
                                editContact();
                                break;
                            case "4":
                                getNumber();
                                break;
                            case "5":
                                addNumber();
                                break;
                            case "6":
                                removeContact();
                                break;
                            case "7":
                                removeNumber();
                                break;
                            case "C":
                                Console.Clear();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        userInput = getUsersInput();
                    }
                    else{
                        Console.WriteLine("\nEnter a valid option");
                        userInput = getUsersInput();

                    }
                }

            }
            catch(Exception e){
                Console.WriteLine(e.Message);
            }
        }

        private static Boolean isInteger(string input){
            int num;
            if(!string.IsNullOrEmpty(input.Trim()) && (int.TryParse(input, out num))){
                return true;
            }
            return false;
        }
        private static void getContact()
        {
            Console.Write("\nContact's name: ");
            string input = Console.ReadLine();
            if(string.IsNullOrEmpty(input.Trim())){
                SqlConnection conn = Connection.connect();
                 string queryString = "select * from contact";
                 SqlCommand command = new SqlCommand (queryString, conn);
                 SqlDataReader reader = command.ExecuteReader();
                 if(reader.Read()){
                    Console.WriteLine("Contacts: ");
                    do
                    {
                        //imprime a informação obtida
                        int id = (int)reader["id"];
                        string name = (string)reader["name"];
                        Console.WriteLine(id+"- "+name);
                    }
                    while( reader.Read());
                    
                }
                else{
                    Console.WriteLine("\nContacts list is empty");
                }
                reader.Close();
            }
            else{
                SqlConnection conn = Connection.connect();
                 string queryString = "select * from contact where name LIKE '%' + @input + '%'";
                 SqlCommand command = new SqlCommand (queryString, conn);
                 command.Parameters.Add( new SqlParameter("@input",input));
                 SqlDataReader reader = command.ExecuteReader();
                 if(reader.Read()){
                    Console.WriteLine("\nContacts: ");
                    do
                    {
                        int id = (int)reader["id"];
                        string name = (string)reader["name"];
                        Console.WriteLine(id+"- "+name);
                    }
                    while( reader.Read());
                    reader.Close();
                
                }
                else{
                    Console.WriteLine("\nNo contact was found");
                    
                }
                reader.Close();
            }
            
            Connection.disconnect();
        }

        private static void editContact()
        {
            Console.WriteLine("\nEDIT CONTACT ");
            Console.Write("\nContact's ID: ");
            string input = Console.ReadLine();
            Console.Write("\nContact's new name: ");
            string newName = Console.ReadLine();
            if(isInteger(input) && (!string.IsNullOrEmpty(newName.Trim()))){                
                 SqlConnection conn = Connection.connect();
                 string queryString = "update contact set name = @newName where id = @input";
                 SqlCommand command = new SqlCommand (queryString, conn);
                 command.Parameters.Add( new SqlParameter("@newName",newName));
                 command.Parameters.Add( new SqlParameter("@input",input));       
                 int registration = command.ExecuteNonQuery();
                 if (registration != 0){
                    Console.WriteLine("\nContact edited successfully");
                 }
                 else{
                    Console.WriteLine("\nContact not found");
                 }
                 Connection.disconnect();
            }
            else{
                Console.WriteLine("\nBoth ID(Only integer(int32) numbers) and New name are required");
            }

        }

        private static void getNumber()
        {
            Console.WriteLine("\nGET NUMBERS");
            Console.Write("\nContact's ID: ");
            string input = Console.ReadLine();

            if(isInteger(input)){
                try{
                    SqlConnection conn = Connection.connect();
                    string queryString = "select c.name, cn.id, cn.number from contact c inner join contactNumber cn on c.id = cn.idContact where cn.idContact = @input";
                    SqlCommand command = new SqlCommand (queryString, conn);
                    command.Parameters.Add( new SqlParameter("@input",input));
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read()){
                        string name = (string)reader["name"];
                        Console.WriteLine(name+"'s numbers: ");
                        do
                        {
                            int id = (int)reader["id"];
                            string number= (string)reader["number"];          
                            Console.WriteLine(id+"- "+number);
                        }
                        while( reader.Read());
                        reader.Close();
                        
                    }
                    else{
                        Console.WriteLine("No contact number found");
                        reader.Close();
                    }
                    Connection.disconnect();
                }
                catch(Exception e){
                    Console.WriteLine(e.Message);
                    
                }
            }
            
            else{
                Console.WriteLine("\nValid ID required (integer number(int 32))");
                
            }
        }

        private static void removeNumber()
        {
            Console.WriteLine("\nREMOVE NUMBERS");
            Console.Write("\nContact's ID: ");
            string input = Console.ReadLine();
            Console.Write("\nContact number's ID: ");
            string idNumber = Console.ReadLine();
            if(isInteger(input) && isInteger(idNumber)){
                try{
                    SqlConnection conn = Connection.connect();
                    string queryString = "delete from contactNumber where id = @idNumber and idContact = @input";
                    SqlCommand command = new SqlCommand (queryString, conn);
                    command.Parameters.Add( new SqlParameter("@input",input));
                    command.Parameters.Add( new SqlParameter("@idNumber",idNumber));
                    int registration = command.ExecuteNonQuery();
                    if(registration != 0){
                        Console.WriteLine("\nContact number removed successfully");
                    }
                    else{
                        Console.WriteLine("\nContact number not found");
                    }
                }
                catch(Exception e){
                    Console.WriteLine(e.Message);
                }
                Connection.disconnect();
                }
  
            
            else{
                Console.WriteLine("\nValid ID's required (integer number(int 32))");
            }
        }

        private static void removeContact()
        {   
            try{
                int num;
                Console.WriteLine("\nREMOVE CONTACT ");
                Console.Write("\nContact's ID: ");
                string input = Console.ReadLine();
                if(isInteger(input)){
                    SqlConnection conn = Connection.connect();
                    string queryString = "delete from contact where id = @input";
                    SqlCommand command = new SqlCommand (queryString, conn);
                    command.Parameters.Add( new SqlParameter("@input",input));
                    int registration = command.ExecuteNonQuery();
                    if(registration != 0){
                        Console.WriteLine("\nContact removed successfully");
                    }
                    else{
                        Console.WriteLine("\nContact not found");
                    }
                    Connection.disconnect();
                }
                
                else{
                    Console.WriteLine("\nValid ID required(integer number(int 32))");
                }
            }
            catch{
                Console.WriteLine("Not possible to remove this contact as it has contact numbers registred");
            }
        }

        private static void addNumber()
        {
            Console.WriteLine("\nADD NUMBER");
            Console.Write("\nContact's ID: ");
            string input = Console.ReadLine();
            Console.Write("\nContact number to add: ");
            string number = Console.ReadLine();
            int num;
            if(isInteger(input) && (!string.IsNullOrEmpty(number.Trim()))){               
                try{
                    SqlConnection conn = Connection.connect();
                    string queryString = "insert into contactNumber (idContact, number) values (@input, @number)";
                    SqlCommand command = new SqlCommand (queryString, conn);
                    command.Parameters.Add( new SqlParameter("@input",input));
                    command.Parameters.Add( new SqlParameter("@number",number));
                    int registration = command.ExecuteNonQuery();
                    Console.WriteLine("\nContact number added successfully");
                }
                catch{
                    Console.WriteLine("\nContact ID not found or invalid number(20 chars max)");
                }
                Connection.disconnect();
            }
            
            else{
                Console.WriteLine("\nBoth ID(Only integer(int32) numbers) and contact number(20 chars max) required");
            }
        }

        private static void addContact()
        {
            try{
            Console.WriteLine("\nADD CONTACT");
            Console.Write("\nContact's name: ");
            string input = Console.ReadLine();
            if(!string.IsNullOrEmpty(input.Trim()) && input.Length < 50 ){
                SqlConnection conn = Connection.connect();
                 string queryString = "insert into contact (name) values (@input)";
                 SqlCommand command = new SqlCommand (queryString, conn);
                 command.Parameters.Add( new SqlParameter("@input",input));
                 int registration = command.ExecuteNonQuery();
                 Console.WriteLine("\nContact added successfully");
                 Connection.disconnect();
            }
            else{
                Console.WriteLine("\nValid name required(50 chars max)");
            }
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
            }
        }
    }           
}