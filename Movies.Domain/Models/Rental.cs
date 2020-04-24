using Movies.Shared;
using System;
using System.Collections.Generic;

namespace Movies.Domain.Models
{
    public class Rental : BaseEntity
    {
        public ApplicationUser User { get; private set; }
        public string UserId { get; private set; }
        public int MovieID { get; private set; }
        public Payment Payment { get; private set; }
        public DateTime RentStartDate { get; private set; }
        public DateTime RentEndDate { get; private set; }
        public bool IsValid { get; private set; }

        public Rental(int rentalId, ApplicationUser user, string userid, int movieid, Payment payment, DateTime rentStartDate, DateTime rentEndDate)
        {
            Id = rentalId;
            User = user;
            UserId = userid;
            MovieID = movieid;
            Payment = payment;
            RentStartDate = rentStartDate;
            RentEndDate = rentEndDate;
        }

        public void SetMovieId(int id)
        {
            this.MovieID = id;
        }
        public void SetUser(ApplicationUser user)
        {
            this.User = user;
        }

        public void SetPayment(Payment payment)
        {
            this.Payment = payment;
        }

        public void SetStartDate()
        {
            this.RentStartDate = DateTime.Now;
        }

        public void SetEndDate(DateTime date)
        {
            this.RentEndDate = date;
        }

        public void SetEndDate()
        {
            this.RentEndDate = this.RentStartDate.AddDays(5);
        }

        public void SetIsValid(bool isValid)
        {
            this.IsValid = isValid;
        }


        public Rental()
        {

        }

        public bool IsRentalValid()
        {
            var actualDate = DateTime.Now;
            if (this.RentEndDate >= actualDate)
            {
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is Rental rental &&
                   Id == rental.Id &&
                   EqualityComparer<ApplicationUser>.Default.Equals(User, rental.User) &&
                   UserId == rental.UserId &&
                   MovieID == rental.MovieID &&
                   EqualityComparer<Payment>.Default.Equals(Payment, rental.Payment) &&
                   RentStartDate == rental.RentStartDate &&
                   RentEndDate == rental.RentEndDate &&
                   IsValid == rental.IsValid;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, User, UserId, MovieID, Payment, RentStartDate, RentEndDate, IsValid);
        }
    }

}
