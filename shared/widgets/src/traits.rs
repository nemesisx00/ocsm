
pub const Signal_SheetUpdated: &'static str = "sheetUpdated";

pub trait CharacterSheet
{
	fn characterName(&self) -> String;
	fn pageName(&self) -> String;
	fn setPageName(&self, name: String);
}
