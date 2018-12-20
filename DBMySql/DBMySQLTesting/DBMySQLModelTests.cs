using DBMySql;
using DBMySql.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace DBMySQLTesting
{
    public class UserModelTest
    {
        [Test]
        public void ReturnTrueIfModelTableFieldsAreCorrect()
        {
            List<string> UserModelFields = MySQLDBStateless.ModelFieldNames(typeof(UserModel));

            StringBuilder StringBuilder = new StringBuilder();

            foreach (string userModelField in UserModelFields)
                StringBuilder.Append(StringBuilder.Length == 0 ? $"{userModelField}" : $", {userModelField}");

            Assert.AreEqual("usrID, usrUID, usrFirstName, usrLastName, usrEmailAddress", StringBuilder.ToString());
        }

        [Test]
        public void ReturnTrueIfModelValuesAreCorrect()
        {
            UserModel UserModel = new UserModel()
            {
                ID = 1,
                UID = "Testert1",
                FirstName = "Test",
                LastName = "Tester",
                EmailAddress = "test@testing.com"
            };

            List<object> UserModelFields = MySQLDBStateless.ModelFieldValues(UserModel);

            StringBuilder StringBuilder = new StringBuilder();

            foreach (object userModelField in UserModelFields)
                StringBuilder.Append(StringBuilder.Length == 0 ? $"{userModelField}" : $", {userModelField}");

            Assert.AreEqual("1, 'Testert1', 'Test', 'Tester', 'test@testing.com'", StringBuilder.ToString());
        }

        [Test]
        public void ReturnTrueIfInsertFieldsAreCorrect()
        {
            UserModel UserModel = new UserModel()
            {
                ID = 1,
                UID = "Testert1",
                FirstName = "Test",
                LastName = "Tester",
                EmailAddress = "test@testing.com"
            };

            string UserInsertFields = MySQLDBStateless.GenerateInsertFields(UserModel);
            Assert.AreEqual("(usrUID, usrFirstName, usrLastName, usrEmailAddress) VALUES ('Testert1', 'Test', 'Tester', 'test@testing.com')", UserInsertFields);
            Assert.AreEqual($"INSERT INTO {UserModel.TableName()} (usrUID, usrFirstName, usrLastName, usrEmailAddress) VALUES ('Testert1', 'Test', 'Tester', 'test@testing.com')", 
                $"INSERT INTO {UserModel.TableName()} {UserInsertFields}");
        }

        [Test]
        public void ReturnTrueIfStandardInsertStatementisCorrect()
        {
            UserModel UserModel = new UserModel()
            {
                ID = 1,
                UID = "Testert1",
                FirstName = "Test",
                LastName = "Tester",
                EmailAddress = "test@testing.com"
            };

            string UserInsertStatement = MySQLDBStateless.GenerateStandardInsertStatement(UserModel);
            Assert.AreEqual($"INSERT INTO {UserModel.TableName()} (usrUID, usrFirstName, usrLastName, usrEmailAddress) VALUES ('Testert1', 'Test', 'Tester', 'test@testing.com')",
                UserInsertStatement);
        }

        [Test]
        public void ReturnTrueIfUpdateFieldsAreCorrect()
        {
            UserModel UserModel = new UserModel()
            {
                ID = 1,
                UID = "Testert1",
                FirstName = "Test",
                LastName = "Tester",
                EmailAddress = "test@testing.com"
            };

            string UserUpdateFields = MySQLDBStateless.GenerateUpdateFields(UserModel);
            Assert.AreEqual("usrUID = 'Testert1', usrFirstName = 'Test', usrLastName = 'Tester', usrEmailAddress = 'test@testing.com'", UserUpdateFields);
            Assert.AreEqual($"UPDATE {UserModel.TableName()} SET usrUID = 'Testert1', usrFirstName = 'Test', usrLastName = 'Tester', usrEmailAddress = 'test@testing.com' WHERE usrID = 1",
                $"UPDATE {UserModel.TableName()} SET {UserUpdateFields} WHERE usrID = 1");
        }

        [Test]
        public void ReturnTrueIfUpdateStatementIsCorrect()
        {
            UserModel UserModel = new UserModel()
            {
                ID = 1,
                UID = "Testert1",
                FirstName = "Test",
                LastName = "Tester",
                EmailAddress = "test@testing.com"
            };

            string UserUpdateStatement = MySQLDBStateless.GenerateStandardUpdateStatement(UserModel, nameof(UserModel.ID), UserModel.ID);
            Assert.AreEqual($"UPDATE {UserModel.TableName()} SET usrUID = 'Testert1', usrFirstName = 'Test', usrLastName = 'Tester', usrEmailAddress = 'test@testing.com' WHERE usrID = 1",
                UserUpdateStatement);
        }

        [Test]
        public void ReturnTrueIfDeleteStatementIsCorrect()
        {
            UserModel UserModel = new UserModel()
            {
                ID = 1,
                UID = "Testert1",
                FirstName = "Test",
                LastName = "Tester",
                EmailAddress = "test@testing.com"
            };

            string UserDeleteStatement = MySQLDBStateless.GenerateStandardDeleteStatement(UserModel, nameof(UserModel.ID), UserModel.ID);
            Assert.AreEqual($"DELETE FROM {UserModel.TableName()} WHERE usrID = 1",
                UserDeleteStatement);
        }

        [Test]
        public void ReturnTrueIfModelTableNameIsCorrect()
        {
            UserModel UserModel = new UserModel();
            Assert.AreEqual("usrUser_usr", UserModel.TableName());
        }
    }
}