use serde::{Deserialize, Serialize};
use crate::gamesystems::GameSystem;

#[derive(Clone, Copy, Debug, Default, Deserialize, Eq, PartialEq, PartialOrd, Ord, Serialize)]
pub struct SheetId
{
	pub index: u32,
	pub game: GameSystem,
}

impl From<(u32, GameSystem)> for SheetId
{
	fn from(value: (u32, GameSystem)) -> Self
	{
		return Self
		{
			index: value.0,
			game: value.1,
		};
	}
}
