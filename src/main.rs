mod app;
mod components;
mod constants;
mod data;
mod gamesystems;
mod io;
mod menu;

use freya::launch::launch_cfg;
use freya::prelude::{LaunchConfig, WindowConfig};
use crate::app::App;
use crate::constants::{AppTitle, BackgroundColor, DefaultWindowSize, MinimumWindowSize};

fn main()
{
	launch_cfg(
		LaunchConfig::new()
			.with_window(
				WindowConfig::new(App)
					.with_background(BackgroundColor)
					.with_min_size(MinimumWindowSize.0, MinimumWindowSize.1)
					.with_size(DefaultWindowSize.0, DefaultWindowSize.1)
					.with_title(AppTitle)
					.with_transparency(false)
			)
	);
}
