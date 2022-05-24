using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Notes.Models
{
    public class OrderTax
    {
        [JsonProperty("tax")]
        public Tax Tax { get; set; }
    }

    public class Tax
    {
        [JsonProperty("amount_to_collect")]
        public double AmountToCollect { get; set; }
        //public Breakdown breakdown { get; set; }
        //public bool freight_taxable { get; set; }
        //public bool has_nexus { get; set; }
        //public Jurisdictions jurisdictions { get; set; }
        //public double order_total_amount { get; set; }
        //public double rate { get; set; }
        //public double shipping { get; set; }
        //public string tax_source { get; set; }
        //public double taxable_amount { get; set; }
    }

    public class Breakdown
    {
        public double city_tax_collectable { get; set; }
        public double city_tax_rate { get; set; }
        public double city_taxable_amount { get; set; }
        public double combined_tax_rate { get; set; }
        public double county_tax_collectable { get; set; }
        public double county_tax_rate { get; set; }
        public double county_taxable_amount { get; set; }
        //public List<LineItem> line_items { get; set; }
        public Shipping shipping { get; set; }
        public double special_district_tax_collectable { get; set; }
        public double special_district_taxable_amount { get; set; }
        public double special_tax_rate { get; set; }
        public double state_tax_collectable { get; set; }
        public double state_tax_rate { get; set; }
        public double state_taxable_amount { get; set; }
        public double tax_collectable { get; set; }
        public double taxable_amount { get; set; }
    }

    public class Jurisdictions
    {
        public string city { get; set; }
        public string country { get; set; }
        public string county { get; set; }
        public string state { get; set; }
    }

    //public class LineItem
    //{
    //    public double city_amount { get; set; }
    //    public double city_tax_rate { get; set; }
    //    public double city_taxable_amount { get; set; }
    //    public double combined_tax_rate { get; set; }
    //    public double county_amount { get; set; }
    //    public double county_tax_rate { get; set; }
    //    public double county_taxable_amount { get; set; }
    //    public string id { get; set; }
    //    public double special_district_amount { get; set; }
    //    public double special_district_taxable_amount { get; set; }
    //    public double special_tax_rate { get; set; }
    //    public double state_amount { get; set; }
    //    public double state_sales_tax_rate { get; set; }
    //    public double state_taxable_amount { get; set; }
    //    public double tax_collectable { get; set; }
    //    public double taxable_amount { get; set; }
    //}

    public class Shipping
    {
        public double city_amount { get; set; }
        public double city_tax_rate { get; set; }
        public double city_taxable_amount { get; set; }
        public double combined_tax_rate { get; set; }
        public double county_amount { get; set; }
        public double county_tax_rate { get; set; }
        public double county_taxable_amount { get; set; }
        public double special_district_amount { get; set; }
        public double special_tax_rate { get; set; }
        public double special_taxable_amount { get; set; }
        public double state_amount { get; set; }
        public double state_sales_tax_rate { get; set; }
        public double state_taxable_amount { get; set; }
        public double tax_collectable { get; set; }
        public double taxable_amount { get; set; }
    }    
}
