using Movies.Shared;
using System;

namespace Movies.Domain.Models
{
    public class Payment : BaseEntity
    {
        public string CardNo { get; set; }
        public decimal Price { get; set; }

        public Payment(int paymentId, string cardNo, decimal price)
        {
            Id = paymentId;
            CardNo = cardNo;
            Price = price;
        }

        public Payment()
        {

        }        

        public void SetCardNo(string cardNo)
        {
            this.CardNo = cardNo;
        }

        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentException();
            }
            this.Price = price;
        }

        public bool CheckLuhn()
        {
            String cardNo = this.CardNo;
            int nDigits = cardNo.Length;

            int nSum = 0;
            bool isSecond = false;
            for (int i = nDigits - 1; i >= 0; i--)
            {
                int d = cardNo[i] - '0';

                if (isSecond == true)
                    d = d * 2;

                // We add two digits to handle 
                // cases that make two digits  
                // after doubling 
                nSum += d / 10;
                nSum += d % 10;

                isSecond = !isSecond;
            }
            return (nSum % 10 == 0);
        }

        //public override bool Equals(object obj)
        //{
        //    return obj is Payment payment &&
        //           Id == payment.Id &&
        //           CardNo == payment.CardNo &&
        //           Price == payment.Price;
        //}

        //public override int GetHashCode()
        //{
        //    return HashCode.Combine(Id, CardNo, Price);
        //}
    }
}
