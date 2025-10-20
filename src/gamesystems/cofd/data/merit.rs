use serde::{Deserialize, Serialize};

#[derive(Clone, Debug, Default, Deserialize, PartialEq, Serialize)]
pub struct Merit
{
	pub name: String,
	pub dots: u32,
}
