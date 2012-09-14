using FileHelpers;

namespace NSTOX.BODataProcessor.Model
{
    [FixedLengthRecord(FixedMode.AllowVariableLength)]
    public class DepartmentFixed
    {
        [FieldFixedLength(4)]
        public string DepartmentId;

        [FieldFixedLength(19)]
        public string Description;
    }
}
