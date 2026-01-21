using Variable.RPG;

namespace Variable.RPG.Tests;

public class RpgStatConditionTests
{
    #region Basic Comparison Operations

    [Fact]
    public void Satisfies_GreaterThan_True()
    {
        // Value = 100, FixedValue = 50
        // 100 > 50 = True
        var stat = new RpgStat(100f);
        var condition = RpgStatCondition.Absolute(StatComparisonOp.GreaterThan, 50f);

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_GreaterThan_False()
    {
        // Value = 30, FixedValue = 50
        // 30 > 50 = False
        var stat = new RpgStat(30f);
        var condition = RpgStatCondition.Absolute(StatComparisonOp.GreaterThan, 50f);

        Assert.False(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_LessThan_True()
    {
        // Value = 20, FixedValue = 50
        // 20 < 50 = True
        var stat = new RpgStat(20f);
        var condition = RpgStatCondition.Absolute(StatComparisonOp.LessThan, 50f);

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_GreaterOrEqual_True_Equal()
    {
        // Value = 50, FixedValue = 50
        // 50 >= 50 = True
        var stat = new RpgStat(50f);
        var condition = RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f);

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_GreaterOrEqual_True_Greater()
    {
        // Value = 75, FixedValue = 50
        // 75 >= 50 = True
        var stat = new RpgStat(75f);
        var condition = RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f);

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_LessOrEqual_True_Equal()
    {
        // Value = 50, FixedValue = 50
        // 50 <= 50 = True
        var stat = new RpgStat(50f);
        var condition = RpgStatCondition.Absolute(StatComparisonOp.LessOrEqual, 50f);

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_Equal_True()
    {
        // Value = 50, FixedValue = 50
        // 50 == 50 = True
        var stat = new RpgStat(50f);
        var condition = RpgStatCondition.Absolute(StatComparisonOp.Equal, 50f);

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_Equal_False()
    {
        // Value = 50.01, FixedValue = 50
        // 50.01 == 50 = False (outside epsilon)
        var stat = new RpgStat(50.01f);
        var condition = RpgStatCondition.Absolute(StatComparisonOp.Equal, 50f);

        Assert.False(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_NotEqual_True()
    {
        // Value = 50.5, FixedValue = 50
        // 50.5 != 50 = True
        var stat = new RpgStat(50.5f);
        var condition = RpgStatCondition.Absolute(StatComparisonOp.NotEqual, 50f);

        Assert.True(stat.Satisfies(condition));
    }

    #endregion

    #region Percentage of Max

    [Fact]
    public void Satisfies_PercentOfMax_LessThan_True()
    {
        // Max = 200, Value = 30, Percentage = 20% (0.2)
        // 30 < (200 * 0.2) = 30 < 40 = True
        var stat = new RpgStat(100f, 0f, 200f);
        stat.Value = 30f; // Direct value set for testing
        var condition = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.2f);

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_PercentOfMax_LessThan_False()
    {
        // Max = 200, Value = 50, Percentage = 20% (0.2)
        // 50 < (200 * 0.2) = 50 < 40 = False
        var stat = new RpgStat(100f, 0f, 200f);
        stat.Value = 50f;
        var condition = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.2f);

        Assert.False(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_PercentOfMax_GreaterOrEqual_True()
    {
        // Max = 200, Value = 180, Percentage = 90% (0.9)
        // 180 >= (200 * 0.9) = 180 >= 180 = True
        var stat = new RpgStat(100f, 0f, 200f);
        stat.Value = 180f;
        var condition = RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterOrEqual, 0.9f);

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_PercentOfMax_GreaterThan_True()
    {
        // Max = 100, Value = 81, Percentage = 80% (0.8)
        // 81 > (100 * 0.8) = 81 > 80 = True
        var stat = new RpgStat(50f, 0f, 100f);
        stat.Value = 81f;
        var condition = RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterThan, 0.8f);

        Assert.True(stat.Satisfies(condition));
    }

    #endregion

    #region Field Checks

    [Fact]
    public void Satisfies_FieldCheck_ModMult_GreaterThan_True()
    {
        // ModMult = 2.5, Target = 2.0
        // 2.5 > 2.0 = True
        var stat = new RpgStat(100f);
        stat.ModMult = 2.5f;
        var condition = RpgStatCondition.FieldCheck(RpgStatField.ModMult, StatComparisonOp.GreaterThan, 2.0f);

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_FieldCheck_ModMult_LessThan_True()
    {
        // ModMult = 0.5, Target = 1.0
        // 0.5 < 1.0 = True
        var stat = new RpgStat(100f);
        stat.ModMult = 0.5f;
        var condition = RpgStatCondition.FieldCheck(RpgStatField.ModMult, StatComparisonOp.LessThan, 1.0f);

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_FieldCheck_Base_NotEqual_Current()
    {
        // Base = 100, Current Value = 156.25 (after modifiers)
        // Base != Value should be true
        var stat = new RpgStat(100f);
        stat.AddModifier(25f, 0.25f); // (100 + 25) * 1.25 = 156.25
        stat.Recalculate(); // Must recalculate to update stat.Value
        var condition = new RpgStatCondition
        {
            FieldToTest = RpgStatField.Base,
            Operator = StatComparisonOp.NotEqual,
            ReferenceSource = StatReferenceSource.Value,
            ReferenceValue = 1.0f
        };

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_FieldCheck_ModAdd_Equal_Zero()
    {
        // ModAdd = 0 (default)
        // 0 == 0 = True
        var stat = new RpgStat(100f);
        var condition = RpgStatCondition.FieldCheck(RpgStatField.ModAdd, StatComparisonOp.Equal, 0f);

        Assert.True(stat.Satisfies(condition));
    }

    #endregion

    #region Reference Sources

    [Fact]
    public void Satisfies_ReferenceSource_Base_Percentage()
    {
        // Base = 100, Current = 60, check if Current < 50% of Base
        // 60 < (100 * 0.5) = 60 < 50 = False
        var stat = new RpgStat(100f);
        stat.Value = 60f;
        var condition = new RpgStatCondition
        {
            FieldToTest = RpgStatField.Value,
            Operator = StatComparisonOp.LessThan,
            ReferenceSource = StatReferenceSource.Base,
            ReferenceValue = 0.5f
        };

        Assert.False(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_ReferenceSource_Base_Percentage_True()
    {
        // Base = 100, Current = 40, check if Current < 50% of Base
        // 40 < (100 * 0.5) = 40 < 50 = True
        var stat = new RpgStat(100f);
        stat.Value = 40f;
        var condition = new RpgStatCondition
        {
            FieldToTest = RpgStatField.Value,
            Operator = StatComparisonOp.LessThan,
            ReferenceSource = StatReferenceSource.Base,
            ReferenceValue = 0.5f
        };

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_ReferenceSource_Value_Percentage()
    {
        // Current = 100, check if Base > 150% of Current
        // Base = 80, Current = 100, 80 > (100 * 1.5) = 80 > 150 = False
        var stat = new RpgStat(80f);
        stat.Value = 100f;
        var condition = new RpgStatCondition
        {
            FieldToTest = RpgStatField.Base,
            Operator = StatComparisonOp.GreaterThan,
            ReferenceSource = StatReferenceSource.Value,
            ReferenceValue = 1.5f
        };

        Assert.False(stat.Satisfies(condition));
    }

    #endregion

    #region Multiple Conditions (AND/OR)

    [Fact]
    public void SatisfiesAll_AllConditionsPass_True()
    {
        // Str >= 50 AND Str >= 30% of Max
        var stat = new RpgStat(75f, 0f, 100f);
        var conditions = new[]
        {
            RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f),
            RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterOrEqual, 0.3f)
        };

        Assert.True(stat.SatisfiesAll(conditions));
    }

    [Fact]
    public void SatisfiesAll_OneConditionFails_False()
    {
        // Str >= 80 (FAIL) AND Str >= 30% of Max (PASS)
        var stat = new RpgStat(75f, 0f, 100f);
        var conditions = new[]
        {
            RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 80f),
            RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterOrEqual, 0.3f)
        };

        Assert.False(stat.SatisfiesAll(conditions));
    }

    [Fact]
    public void SatisfiesAny_OneConditionPasses_True()
    {
        // Str >= 80 (FAIL) OR Str >= 70% of Max (PASS)
        var stat = new RpgStat(75f, 0f, 100f);
        var conditions = new[]
        {
            RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 80f),
            RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterOrEqual, 0.7f)
        };

        Assert.True(stat.SatisfiesAny(conditions));
    }

    [Fact]
    public void SatisfiesAny_AllConditionsFail_False()
    {
        // Str >= 80 (FAIL) OR Str >= 90% of Max (FAIL)
        var stat = new RpgStat(75f, 0f, 100f);
        var conditions = new[]
        {
            RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 80f),
            RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterOrEqual, 0.9f)
        };

        Assert.False(stat.SatisfiesAny(conditions));
    }

    [Fact]
    public void SatisfiesAny_EmptyArray_False()
    {
        var stat = new RpgStat(75f);
        var conditions = Array.Empty<RpgStatCondition>();

        Assert.False(stat.SatisfiesAny(conditions));
    }

    #endregion

    #region Count Satisfied

    [Fact]
    public void CountSatisfied_TwoOfThree_Pass()
    {
        // 3 conditions, expect 2 to pass
        var stat = new RpgStat(60f, 0f, 100f);
        var conditions = new[]
        {
            RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f), // PASS
            RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 70f), // FAIL
            RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterOrEqual, 0.5f) // PASS (60 >= 50)
        };

        var count = stat.CountSatisfied(conditions);
        Assert.Equal(2, count);
    }

    [Fact]
    public void SatisfiesCount_ExactMatch_True()
    {
        // Exactly 2 of 3 conditions should pass
        var stat = new RpgStat(60f, 0f, 100f);
        var conditions = new[]
        {
            RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f), // PASS
            RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 70f), // FAIL
            RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterOrEqual, 0.5f) // PASS
        };

        Assert.True(stat.SatisfiesCount(conditions, 2));
        Assert.False(stat.SatisfiesCount(conditions, 3));
    }

    [Fact]
    public void SatisfiesAtLeast_MinimumRequired()
    {
        // At least 2 of 3 conditions should pass
        var stat = new RpgStat(60f, 0f, 100f);
        var conditions = new[]
        {
            RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 50f), // PASS
            RpgStatCondition.Absolute(StatComparisonOp.GreaterOrEqual, 70f), // FAIL
            RpgStatCondition.PercentOfMax(StatComparisonOp.GreaterOrEqual, 0.5f) // PASS
        };

        Assert.True(stat.SatisfiesAtLeast(conditions, 2));
        Assert.True(stat.SatisfiesAtLeast(conditions, 1)); // 2 >= 1
        Assert.False(stat.SatisfiesAtLeast(conditions, 3)); // 2 < 3
    }

    #endregion

    #region Convenience Methods

    [Fact]
    public void BelowPercentOfMax_CreatesCorrectCondition()
    {
        var stat = new RpgStat(100f, 0f, 200f);
        stat.Value = 30f;
        var condition = stat.BelowPercentOfMax(0.2f);

        Assert.Equal(RpgStatField.Value, condition.FieldToTest);
        Assert.Equal(StatComparisonOp.LessThan, condition.Operator);
        Assert.Equal(StatReferenceSource.Max, condition.ReferenceSource);
        Assert.Equal(0.2f, condition.ReferenceValue);
        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void AbovePercentOfMax_CreatesCorrectCondition()
    {
        var stat = new RpgStat(100f, 0f, 200f);
        stat.Value = 190f;
        var condition = stat.AbovePercentOfMax(0.9f);

        Assert.Equal(RpgStatField.Value, condition.FieldToTest);
        Assert.Equal(StatComparisonOp.GreaterOrEqual, condition.Operator);
        Assert.Equal(StatReferenceSource.Max, condition.ReferenceSource);
        Assert.Equal(0.9f, condition.ReferenceValue);
        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void AtLeast_CreatesCorrectCondition()
    {
        var stat = new RpgStat(60f);
        var condition = stat.AtLeast(50f);

        Assert.Equal(RpgStatField.Value, condition.FieldToTest);
        Assert.Equal(StatComparisonOp.GreaterOrEqual, condition.Operator);
        Assert.Equal(StatReferenceSource.FixedValue, condition.ReferenceSource);
        Assert.Equal(50f, condition.ReferenceValue);
        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void AtMost_CreatesCorrectCondition()
    {
        var stat = new RpgStat(40f);
        var condition = stat.AtMost(50f);

        Assert.Equal(RpgStatField.Value, condition.FieldToTest);
        Assert.Equal(StatComparisonOp.LessOrEqual, condition.Operator);
        Assert.Equal(StatReferenceSource.FixedValue, condition.ReferenceSource);
        Assert.Equal(50f, condition.ReferenceValue);
        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void MultiplierAbove_CreatesCorrectCondition()
    {
        var stat = new RpgStat(100f);
        stat.ModMult = 2.5f;
        var condition = stat.MultiplierAbove(2.0f);

        Assert.Equal(RpgStatField.ModMult, condition.FieldToTest);
        Assert.Equal(StatComparisonOp.GreaterThan, condition.Operator);
        Assert.Equal(2.0f, condition.ReferenceValue);
        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void MultiplierBelow_CreatesCorrectCondition()
    {
        var stat = new RpgStat(100f);
        stat.ModMult = 0.5f;
        var condition = stat.MultiplierBelow(1.0f);

        Assert.Equal(RpgStatField.ModMult, condition.FieldToTest);
        Assert.Equal(StatComparisonOp.LessThan, condition.Operator);
        Assert.Equal(1.0f, condition.ReferenceValue);
        Assert.True(stat.Satisfies(condition));
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Satisfies_InvalidField_ReturnsFalse()
    {
        var stat = new RpgStat(100f);
        var condition = new RpgStatCondition
        {
            FieldToTest = RpgStatField.None,
            Operator = StatComparisonOp.GreaterThan,
            ReferenceSource = StatReferenceSource.FixedValue,
            ReferenceValue = 50f
        };

        Assert.False(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_ZeroMax_PercentageWorks()
    {
        // Edge case: Max is 0, percentage check
        var stat = new RpgStat(100f, 0f, 0f);
        stat.Value = 10f;
        var condition = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.5f);

        // 10 < (0 * 0.5) = 10 < 0 = False
        Assert.False(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_NegativeValue_Works()
    {
        var stat = new RpgStat(100f, -200f, 200f); // Allow negative
        stat.Value = -50f;
        var condition = RpgStatCondition.Absolute(StatComparisonOp.LessThan, 0f);

        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_FloatPrecision_EpsilonHandling()
    {
        // Test epsilon handling for float equality
        var stat = new RpgStat(100f);
        stat.Value = 50.000005f;
        var condition = RpgStatCondition.Absolute(StatComparisonOp.Equal, 50f);

        // Within epsilon (0.00001), should return true
        Assert.True(stat.Satisfies(condition));
    }

    [Fact]
    public void Satisfies_FloatPrecision_OutsideEpsilon()
    {
        // Test epsilon handling for float equality
        var stat = new RpgStat(100f);
        stat.Value = 50.01f;
        var condition = RpgStatCondition.Absolute(StatComparisonOp.Equal, 50f);

        // Outside epsilon, should return false
        Assert.False(stat.Satisfies(condition));
    }

    #endregion

    #region Description and Display

    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        var condition = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.2f);
        var str = condition.ToString();

        Assert.Contains("Value", str);
        Assert.Contains("<", str);
        Assert.Contains("Max", str);
        Assert.Contains("20.0%", str);
    }

    [Fact]
    public void DescribeCondition_ReturnsCorrectDescription()
    {
        var stat = new RpgStat(100f, 0f, 200f);
        stat.Value = 30f;
        var condition = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.2f);

        var description = stat.DescribeCondition(condition);

        Assert.Contains("Value", description);
        Assert.Contains("30", description);
        Assert.Contains("<", description);
        Assert.Contains("PASS", description);
    }

    [Fact]
    public void DescribeCondition_FailedCondition_ReturnsFail()
    {
        var stat = new RpgStat(100f, 0f, 200f);
        stat.Value = 50f;
        var condition = RpgStatCondition.PercentOfMax(StatComparisonOp.LessThan, 0.2f);

        var description = stat.DescribeCondition(condition);

        Assert.Contains("FAIL", description);
    }

    #endregion

    #region Equality and Hashing

    [Fact]
    public void Equality_SameConditions_AreEqual()
    {
        var condition1 = RpgStatCondition.Absolute(StatComparisonOp.GreaterThan, 50f);
        var condition2 = RpgStatCondition.Absolute(StatComparisonOp.GreaterThan, 50f);

        Assert.Equal(condition1, condition2);
        Assert.True(condition1 == condition2);
    }

    [Fact]
    public void Equality_DifferentValues_NotEqual()
    {
        var condition1 = RpgStatCondition.Absolute(StatComparisonOp.GreaterThan, 50f);
        var condition2 = RpgStatCondition.Absolute(StatComparisonOp.GreaterThan, 75f);

        Assert.NotEqual(condition1, condition2);
        Assert.True(condition1 != condition2);
    }

    [Fact]
    public void Equality_DifferentOperators_NotEqual()
    {
        var condition1 = RpgStatCondition.Absolute(StatComparisonOp.GreaterThan, 50f);
        var condition2 = RpgStatCondition.Absolute(StatComparisonOp.LessThan, 50f);

        Assert.NotEqual(condition1, condition2);
    }

    [Fact]
    public void GetHashCode_EqualConditions_SameHash()
    {
        var condition1 = RpgStatCondition.Absolute(StatComparisonOp.GreaterThan, 50f);
        var condition2 = RpgStatCondition.Absolute(StatComparisonOp.GreaterThan, 50f);

        Assert.Equal(condition1.GetHashCode(), condition2.GetHashCode());
    }

    #endregion

    #region Extension Methods Tests

    [Fact]
    public void StatComparisonOpExtensions_GetSymbol_ReturnsCorrectSymbol()
    {
        Assert.Equal("==", StatComparisonOp.Equal.GetSymbol());
        Assert.Equal("!=", StatComparisonOp.NotEqual.GetSymbol());
        Assert.Equal(">", StatComparisonOp.GreaterThan.GetSymbol());
        Assert.Equal(">=", StatComparisonOp.GreaterOrEqual.GetSymbol());
        Assert.Equal("<", StatComparisonOp.LessThan.GetSymbol());
        Assert.Equal("<=", StatComparisonOp.LessOrEqual.GetSymbol());
    }

    [Fact]
    public void StatComparisonOpExtensions_GetDisplayName_ReturnsCorrectName()
    {
        Assert.Equal("Equals", StatComparisonOp.Equal.GetDisplayName());
        Assert.Equal("Greater Than", StatComparisonOp.GreaterThan.GetDisplayName());
    }

    [Fact]
    public void StatReferenceSourceExtensions_GetDisplayName_ReturnsCorrectName()
    {
        Assert.Equal("Fixed Value", StatReferenceSource.FixedValue.GetDisplayName());
        Assert.Equal("Max", StatReferenceSource.Max.GetDisplayName());
        Assert.Equal("Base", StatReferenceSource.Base.GetDisplayName());
    }

    [Fact]
    public void StatReferenceSourceExtensions_IsPercentage_ReturnsCorrectValue()
    {
        Assert.False(StatReferenceSource.FixedValue.IsPercentage());
        Assert.True(StatReferenceSource.Max.IsPercentage());
        Assert.True(StatReferenceSource.Base.IsPercentage());
        Assert.True(StatReferenceSource.Value.IsPercentage());
    }

    #endregion
}
