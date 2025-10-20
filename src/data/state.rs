use freya::prelude::{fc_to_builder, rsx, Element, GlobalSignal, IntoDynNode,
	Readable};
use serde::{Deserialize, Serialize};
use crate::data::SheetId;
use crate::gamesystems::{CofdSheet, CofdSheetElement, GameSystem, Vtr2eSheet,
	Vtr2eSheetElement};

#[derive(Clone, Debug, Default, Deserialize, Serialize)]
pub struct AppStateData
{
	pub activeId: Option<SheetId>,
	
	#[serde(skip)]
	pub menuOpen: bool,
	
	#[serde(skip)]
	pub sheets: SheetData,
}

impl AppStateData
{
	pub fn updateActiveId(&mut self, id: Option<SheetId>)
	{
		match self.sheets.len()
		{
			1 => self.activeId = id,
			_ => self.activeId = None,
		}
	}
}

#[derive(Clone, Debug, Default)]
pub struct SheetData
{
	pub cofd: Vec<CofdSheet>,
	pub vtr2e: Vec<Vtr2eSheet>,
}

impl SheetData
{
	pub fn addSheet(&mut self, game: GameSystem)
	{
		match game
		{
			GameSystem::Cofd => self.cofd.push(CofdSheet
			{
				id: (self.cofd.len() as u32, game).into(),
				..Default::default()
			}),
			
			GameSystem::Vtr2e => self.vtr2e.push(Vtr2eSheet
			{
				id: (self.vtr2e.len() as u32, game).into(),
				..Default::default()
			}),
			
			GameSystem::None => {},
		}
	}
	
	pub fn generateElement(&self, id: SheetId) -> Element
	{
		return match id.game
		{
			GameSystem::Cofd => match self.cofd.iter().find(|s| s.id == id)
			{
				None => rsx!(),
				Some(sheet) => rsx!(CofdSheetElement { id: sheet.id }),
			},
			
			GameSystem::Vtr2e => match self.vtr2e.iter().find(|s| s.id == id)
			{
				None => rsx!(),
				Some(sheet) => rsx!(Vtr2eSheetElement { id: sheet.id }),
			},
			
			GameSystem::None => rsx!(),
		};
	}
	
	pub fn getLastSheetId(&self, game: GameSystem) -> Option<SheetId>
	{
		return match game
		{
			GameSystem::Cofd => Some(self.cofd.last()?.id),
			GameSystem::Vtr2e => Some(self.vtr2e.last()?.id),
			GameSystem::None => None,
		};
	}
	
	pub fn len(&self) -> usize
	{
		return self.cofd.len()
			+ self.vtr2e.len();
	}
}
