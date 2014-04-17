using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using CAPITAL.ORM;
using CAPITAL.ORM.Exceptions;
using CAPITAL.ORM.Objects;
using CAPITAL.Engine.Interfaces;

namespace CAPITAL.Engine.Managers
{
    public class UserManagement : Base, IUserManagement
    {
        public User CreateUser(User user)
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    if (context.Users.Where(x => x.Email.ToLower() == user.Email.ToLower()).FirstOrDefault() == null)
                    {
                        user.CreationDate = DateTime.Now;
                        user.LastAccessDate = DateTime.Now;

                        ValidationContext valContext = new ValidationContext(this, null, null);
                        var errors = user.Validate(valContext);

                        if (errors.Count() == 0)
                        {
                            context.Users.Add(user);
                            context.SaveChanges();
                            return context.Users.FirstOrDefault(x => x.Email == user.Email);
                        }
                        else
                            throw new ModelException(errors);
                    }
                    else
                    {
                        throw new ModelException("Email Address Already Exists!");
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                throw new ModelException(ex);
            }
            catch (DbUnexpectedValidationException ex)
            {
                throw new ModelException(ex);
            }
            catch (ModelException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LogError(user, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        public User Login(User user)
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    var checkUser = context.Users
                            .Where(x =>
                                x.Email.ToLower().Equals(user.Email.ToLower()) &&
                                x.Password.Equals(user.Password))
                            .FirstOrDefault();

                    if (checkUser != null)
                    {
                        if (user.Devices != null)
                        {
                            if (user.Devices.Count > 0)
                            {
                                Device temp = user.Devices.SingleOrDefault();

                                if (context.Devices.Where(x => x.UserId == checkUser.UserId && x.UniqueDeviceId == temp.UniqueDeviceId).FirstOrDefault() == null)
                                {
                                    // New Phone
                                    temp.UserId = checkUser.UserId;
                                    temp.CreationDate = DateTime.Now;
                                    context.Devices.Add(temp);
                                    context.SaveChanges();
                                }
                            }
                        }

                        checkUser.LastAccessDate = DateTime.Now;
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new ModelException("Invalid Email Address and/or Password!");
                    }

                    return checkUser;
                }
            }
            catch (ModelException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LogError(user, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }
    }
}
