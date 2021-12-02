using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoodWeb.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int Qty { get; set; }
        public int Unit_Price { get; set; }
        public float Order_Bill { get; set; }
        public DateTime? Order_Date { get; set; }

        public int? FkProdId { get; set; }
        [ForeignKey("FkProdId")]
        public virtual Products prodcts { get; set; }

        public int? FkInvoiceID { get; set; }
        [ForeignKey("FkInvoiceID")]
        public virtual InvoiceModel invoices { get; set; }


    }
    
    public class latLng
    {
        public float lat { get; set; }
        public float lng { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string LocDet { get; set; }
    }

    public class Rootobject
    {
        public Info info { get; set; }
        public Options options { get; set; }
        public Result[] results { get; set; }
    }

    public class Info
    {
        public int statuscode { get; set; }
        public Copyright copyright { get; set; }
        public object[] messages { get; set; }
    }

    public class Copyright
    {
        public string text { get; set; }
        public string imageUrl { get; set; }
        public string imageAltText { get; set; }
    }

    public class Options
    {
        public int maxResults { get; set; }
        public bool thumbMaps { get; set; }
        public bool ignoreLatLngInput { get; set; }
    }

    public class Result
    {
        public Providedlocation providedLocation { get; set; }
        public Location[] locations { get; set; }
    }

    public class Providedlocation
    {
        public string location { get; set; }
    }

    public class Location
    {
        public string street { get; set; }
        public string adminArea6 { get; set; }
        public string adminArea6Type { get; set; }
        public string adminArea5 { get; set; }
        public string adminArea5Type { get; set; }
        public string adminArea4 { get; set; }
        public string adminArea4Type { get; set; }
        public string adminArea3 { get; set; }
        public string adminArea3Type { get; set; }
        public string adminArea1 { get; set; }
        public string adminArea1Type { get; set; }
        public string postalCode { get; set; }
        public string geocodeQualityCode { get; set; }
        public string geocodeQuality { get; set; }
        public bool dragPoint { get; set; }
        public string sideOfStreet { get; set; }
        public string linkId { get; set; }
        public string unknownInput { get; set; }
        public string type { get; set; }
        public Latlng latLng { get; set; }
        public Displaylatlng displayLatLng { get; set; }
        public string mapUrl { get; set; }
    }

    public class Latlng
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Displaylatlng
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

}