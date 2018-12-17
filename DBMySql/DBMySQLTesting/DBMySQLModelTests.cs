using DBMySql;
using DBMySql.Models;
using NUnit.Framework;

namespace DBMySQLTesting
{
    public class UserModelTest
    {
        [Test]
        public void ReturnTrueIfModelTableFieldsAreCorrect()
        {
            string UserModelFIelds = MySQLDBStateless.ModelFieldNames(typeof(UserModel));
            Assert.AreEqual("usrID, usrUID, usrFirstName, usrLastName, usrEmailAddress", UserModelFIelds);
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

            string UserModelFIelds = MySQLDBStateless.ModelFieldValues(UserModel);
            Assert.AreEqual("1, 'Testert1', 'Test', 'Tester', 'test@testing.com'", UserModelFIelds);
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

            string UserUpdateFields = MySQLDBStateless.GenerateInsertFields(UserModel);
            Assert.AreEqual("(usrUID, usrFirstName, usrLastName, usrEmailAddress) VALUES ('Testert1', 'Test', 'Tester', 'test@testing.com')", UserUpdateFields);
            Assert.AreEqual($"INSERT INTO {UserModel.TableName()} (usrUID, usrFirstName, usrLastName, usrEmailAddress) VALUES ('Testert1', 'Test', 'Tester', 'test@testing.com')", 
                $"INSERT INTO {UserModel.TableName()} {UserUpdateFields}");
        }

        [Test]
        public void ReturnTrueIfModelTableNameIsCorrect()
        {
            UserModel UserModel = new UserModel();
            Assert.AreEqual("usrUser_usr", UserModel.TableName());
        }
    }
}