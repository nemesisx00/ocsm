#[derive(Copy, Clone)]
pub enum StatefulMode
{
	CircleOne,
	CircleThree,
	BoxOne,
	BoxTwo,
	BoxThree,
}

impl From<u32> for StatefulMode
{
	fn from(value: u32) -> Self
	{
		return match value
		{
			1 => StatefulMode::CircleThree,
			2 => StatefulMode::BoxOne,
			3 => StatefulMode::BoxTwo,
			4 => StatefulMode::BoxThree,
			_ => StatefulMode::CircleOne,
		};
	}
}

impl From<StatefulMode> for u32
{
	fn from(value: StatefulMode) -> Self
	{
		return match value
		{
			StatefulMode::CircleThree => 1,
			StatefulMode::BoxOne => 2,
			StatefulMode::BoxTwo => 3,
			StatefulMode::BoxThree => 4,
			_ => 0,
		};
	}
}

impl StatefulMode
{
	pub fn getMaxStates(mode: Self) -> u32
	{
		return match mode
		{
			StatefulMode::CircleOne => 2,
			StatefulMode::CircleThree => 4,
			StatefulMode::BoxOne => 2,
			StatefulMode::BoxTwo => 3,
			StatefulMode::BoxThree => 4,
		};
	}
}
