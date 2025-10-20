mod id;
mod state;
mod traits;

use freya::prelude::{GlobalSignal, Signal};
use state::AppStateData;

pub use id::SheetId;
pub use traits::CharacterSheet;

pub static AppState: GlobalSignal<AppStateData> = Signal::global(|| Default::default());
