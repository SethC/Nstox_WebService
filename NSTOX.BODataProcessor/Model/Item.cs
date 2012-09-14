using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;

namespace NSTOX.BODataProcessor.Model
{

    [FixedLengthRecord(FixedMode.AllowVariableLength)]
    public class ItemFixed
    {
        [FieldFixedLength(14)]
        public string ItemID;

        [FieldFixedLength(4)]
        public string DepartmentID;

        [FieldFixedLength(21)]
        public string ReceiptDescription;

        [FieldFixedLength(2)]
        public string TaxRateFlag1;

        [FieldFixedLength(2)]
        public string TaxRateFlag2;

        [FieldFixedLength(2)]
        public string TaxRateFlag3;

        [FieldFixedLength(2)]
        public string TaxRateFlag4;

        [FieldFixedLength(2)]
        public string TaxRateFlag5;

        [FieldFixedLength(2)]
        public string TaxRateFlag6;

        [FieldFixedLength(2)]
        public string TaxRateFlag7;

        [FieldFixedLength(2)]
        public string TaxRateFlag8;

        [FieldFixedLength(2)]
        public string FoodStampFlag;

        [FieldFixedLength(11)]
        public string NoneMerchandiseID;

        [FieldFixedLength(2)]
        public string DecimalQuantityFlag;

        [FieldFixedLength(2)]
        public string NegativeEntryFlag;

        [FieldFixedLength(2)]
        public string StoreCouponFlag;

        [FieldFixedLength(2)]
        public string VendorCouponFlag;

        [FieldFixedLength(2)]
        public string WICFlag;

        [FieldFixedLength(2)]
        public string AssumeQuantityFlag;

        [FieldFixedLength(2)]
        public string SaleAuthorizeFlag;

        [FieldFixedLength(2)]
        public string ManualPriceFlag;

        [FieldFixedLength(2)]
        public string QuantityRequiredFlag;

        [FieldFixedLength(2)]
        public string WeightedItemFlag;

        [FieldFixedLength(2)]
        public string InhibitQuantityFlag;

        [FieldFixedLength(2)]
        public string NoneDiscountFlag;

        [FieldFixedLength(2)]
        public string BonusCouponFlag;

        [FieldFixedLength(2)]
        public string CostPlusFlag;

        [FieldFixedLength(2)]
        public string PriceVerifyFlag;

        [FieldFixedLength(2)]
        public string PriceOverrideFlag;

        [FieldFixedLength(2)]
        public string SupplierPromotionFlag;

        [FieldFixedLength(2)]
        public string SaveDiscountFlag;

        [FieldFixedLength(2)]
        public string ItemOnSaleFlag;

        [FieldFixedLength(10)]
        public string RetailPrice;

        [FieldFixedLength(10)]
        public string CenterPrice;

        [FieldFixedLength(3)]
        public string UnitQuantity;

        [FieldFixedLength(4)]
        public string ReturnCode;

        [FieldFixedLength(4)]
        public string FamilyId;

        [FieldFixedLength(4)]
        public string DiscountId;

        [FieldFixedLength(11)]
        public string QDXFrequentShopperValue;

        [FieldFixedLength(4)]
        public string FrequentShopperType;

        [FieldFixedLength(4)]
        public string TareWeightNumber;

        [FieldFixedLength(14)]
        public string InternalId;

        [FieldFixedLength(4)]
        public string SecondFamilyId;

        [FieldFixedLength(4)]
        public string POSMessageId;

        [FieldFixedLength(3)]
        public string SaleRestrictionId;

        [FieldFixedLength(4)]
        public string FrequentShopperLimit;

        [FieldFixedLength(6)]
        public string ComparisonType;

        [FieldFixedLength(10)]
        public string ComparisonPrice;

        [FieldFixedLength(10)]
        public string ComparisonQuantity;

        [FieldFixedLength(11)]
        public string ComparisonDate;

        [FieldFixedLength(11)]
        public string DEAGroup;

        [FieldFixedLength(6)]
        public string MixMatchIdFiveDigit;

        [FieldFixedLength(2)]
        public string ExcludeMinimumPurchaseFlag;

        [FieldFixedLength(11)]
        public string ItemPoints;

        [FieldFixedLength(6)]
        public string PriceGroupId;

        [FieldFixedLength(10)]
        public string OriginalPrice;

        [FieldFixedLength(4)]
        public string OriginalUnitQuantity;

        [FieldFixedLength(2)]
        public string SSPProductFlag;

        [FieldFixedLength(2)]
        public string SupervisorAuthorizationRequiredFlag;

        [FieldFixedLength(9)]
        public string WICCVVFlag;

        [FieldFixedLength(21)]
        public string Unknown01;

        [FieldFixedLength(6)]
        public string Size;

        [FieldFixedLength(5)]
        public string Unit;

    }

}
