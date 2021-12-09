using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
namespace HSBM.Common.Enums
{

    public enum UserTypes
    {
        Admin = 1,
        SubAdmin = 2,
        User = 3,
        SubUser = 4,
        Customer = 5
    };

    public enum DefaultRole
    {
        Admin = 1,
        NonSubscriber = 2,
        General = 3
    };

    public enum FilterType
    {
        Contains,
        Equals
    }

    public enum Module
    {
        SystemUsers = 1,
        Roles = 2,
        MultiCurrency = 3,
        Amenity = 4,
        Banners = 5,
        EmailTemplates = 6,
        SiteSettings = 7,
        CMSPages = 8,
        Customers = 9,
        Subscribers = 10,
        FarmStays = 11,
        FarmStaysRatingsReviews = 12,
        Inventory = 13,
        Blogs = 14,
        BlogsComment = 15,
        Discounts = 16,
        AccountStatement = 17,
        Outstanding = 18,
        Orders = 19,
        Category = 20,
        Home = 21
        //Users = 1,
        //Role = 2,
        //SendEmail = 3,
        //Location = 4,
        //Category = 5,
        //Banner = 6,
        //CMSPages = 7,
        //SiteSettings = 8,
        //LanguageMaster = 9,
        //Hotel = 11,
        //Vehicle = 12,
        //Tour = 13,
        //Currency = 14,
        //VehicleAdmin = 15,
        //Subscription = 16,
        //TourCategory = 17,
        //TourSubCategory = 18,
        //TourAdmin = 19,
        //Reports = 20,
        //Bookings = 21,
        //POI = 22,
        //SupplierMarkup = 23,
        //SupplierUsers = 24,
        //SupplierRole = 25,
        //Tax = 26,
        //SupplierBookings = 27,
        //Transfer = 28,
        //AmenityMaster = 29,
        //FarmStays = 30,
        //CategoryMaster = 29,
    }

    public enum ModuleAccess
    {
        CanView = 1,
        CanAdd = 2,
        CanUpdate = 3,
        CanDelete = 4
    }

    public enum EmailTemplateType
    {
        [Description("Verification")]
        Verification = 1,
        [Description("Forgot Password")]
        ForgotPassword = 2,
        [Description("Change Password")]
        ChangePassword = 3,
        [Description("Booking Confirmation")]
        BookingConfirmation = 4,
        [Description("Checkout Email")]
        CheckoutEmail = 5,
        [Description("Thank You")]
        ThankYou = 6,
        [Description("Ratings & Reviews")]
        ReviewAndRatings = 7,
        [Description("Subscription")]
        Subscription = 8,
        [Description("Booking Cancellation")]
        BookingCancellation = 9,
        [Description("Admin Booking Confirmation")]
        AdminBookingConfirmation = 10,
        [Description("Admin Booking Cancellation")]
        AdminBookingCancellation = 11,
        [Description("Customer Registration")]
        CustomerRegistration = 12
    }

    public enum RoomType
    {
        [Description("Single")]
        Single = 1,
        [Description("Double")]
        Double = 2,
        [Description("Triple")]
        Triple = 3
    }

    public enum NotificationType
    {
        Admin = 1,
    }



    public class TextValueAttribute : System.Attribute
    {
        public string Text;
        public TextValueAttribute(string text) { Text = text; }
    }

    public enum PaymentType
    {
        Web = 1,
        Manual = 2
    }

    public enum PaymentGateways
    {
        PayPal = 1,
        CCAvenue = 2,
        SagePay = 3,
        AuthorizeNet = 4,
    }

    public enum ResponseType
    {
        Unknown,
        Ok,
        NotAuthed,
        Abort,
        Rejected,
        Authenticated,
        Registered,
        Malformed,
        Error,
        Invalid,
    }

    public enum BookingType
    {
        Hotel
    }
    public enum HotelbedsCustomerType
    {
        AD, CH
    }
    public enum ShowDirectPaymentType
    {
        [Description("S")]
        AT_HOTEL,
        [Description("N")]
        AT_WEB,
        [Description("A")]
        BOTH
    }



    public enum AgeBand
    {
        [Description("ADULT")]
        A,
        [Description("CHILD")]
        C
    }

    public enum BookingStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Failed")]
        Failed = 2,
        [Description("Confirmed")]
        CONFIRMED = 3,
        [Description("Cancelled")]
        Cancel = 4
    }

    public enum MarkupType
    {
        Percentage = 1,
        [Description("Flat Amount")]
        FlatAmount = 2
    }

    public enum FacilityType
    {
        Hotel = 1,
        Bedroom = 2
    }

    public enum Languages
    {
        English,
        French
    }

    public enum MealTypes
    {
        Breakfast = 1,
        Lunch = 2,
        Dinner = 3
    }



    public enum RateType
    {
        Percentage = 1,
        Flat = 2
    }

    public enum SiteSettingEnum
    {
        [Description("Facebook URL")]
        FacebookURL = 1,
        [Description("Twitter URL")]
        TwitterURL = 2,
        [Description("Youtube URL")]
        YoutubeURL = 3,
        [Description("Copyright Text")]
        CopyrightText = 4,
        [Description("Default Currency")]
        DefaultCurrency = 5,
        [Description("Google Plus")]
        GooglePlus = 6,
        [Description("Slider Header")]
        SliderHeader = 7,
        [Description("Slider Title")]
        SliderTitle = 8,
        [Description("Pinterest URL")]
        PinterestURL = 9
    }

    public enum CMSPages
    {
        [Description("About Us")]
        AboutUs = 1,
        [Description("Terms & Conditions")]
        TermsAndConditions = 2,
        [Description("Privacy Policy")]
        PrivacyPolicy = 3,
        [Description("FAQs")]
        FAQs = 4,
        [Description("Home Page Description")]
        HomePageDescription = 5
    }

    //public enum OrderStatus
    //{
    //    [Description("PENDING")]
    //    PENDING = 1,
    //    [Description("CONFIRM")]
    //    CONFIRM = 2,
    //    [Description("COMPLETE")]
    //    COMPLETE = 3,
    //    [Description("CANCEL")]
    //    CANCEL = 4
    //}

    public enum GraphFilter
    {
        [Description("Day")]
        Day = 1,
        [Description("Week")]
        Week = 2,
        [Description("Month")]
        Month = 3,
        [Description("Year")]
        Year = 4,
        [Description("Custom")]
        Custom = 5
    }

    public enum RatePlanEnum
    {
        [Description("MP1")]
        MP1 = 1,
        [Description("MP2")]
        MP2 = 2,
        [Description("MP3")]
        MP3 = 3,
        [Description("MP4")]
        MP4 = 4,
        [Description("MP5")]
        MP5 = 5
    }

    public enum PlanEnum
    {
        [Description("Single")]
        Price = 1,
        [Description("Double")]
        Double = 2,
        [Description("Triple")]
        Triple = 3,
        [Description("ExtraBed")]
        ExtraBed = 4,
        [Description("ExtraChild")]
        ExtraChild = 5,
        [Description("ExtraAdult")]
        ExtraAdult = 6
    }

    public enum AxisPaid
    {
        [Description("True")]
        True = 1,
        [Description("False")]
        False = 2,
        [Description("Partial")]
        Partial = 3
    }

}
