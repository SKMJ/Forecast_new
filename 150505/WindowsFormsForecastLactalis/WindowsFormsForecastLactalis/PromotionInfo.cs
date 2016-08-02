using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsForecastLactalis
{
    public class PromotionInfo
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string ItemNumber { get; set; }
        public int Week { get; set; }
        public int Quantity { get; set; }
        public string Division { get; set; }

        private string _Chain;
        public string Chain {
            get { return this._Chain; }
            set
            {
                this._Chain = value;
                this.PromotionType = PromotionTypeEnum.CHAIN;
            }
        }

        private string _Customer;
        public string Customer {
            get { return this._Customer; }
            set
            {
                this._Customer = value;
                this.PromotionType = PromotionTypeEnum.CUSTOMER;
            }
        }

        public PromotionTypeEnum PromotionType { get; protected set; }


        public PromotionInfo()
        {
            PromotionType = PromotionTypeEnum.NOT_SET;
        }

        public enum PromotionTypeEnum { CHAIN, CUSTOMER, NOT_SET};
    }
}
