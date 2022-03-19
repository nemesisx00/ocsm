#![allow(non_snake_case, non_upper_case_globals)]

pub(crate) mod core;
pub(crate) mod wod;

use crate::core::components::app::App;
use dioxus::desktop::{
	tao::dpi::LogicalSize,
	launch_cfg
};

fn main()
{
    launch_cfg(App, |cfg|
	{
		cfg.with_window(|w|
		{
			w.with_title("Open Character Sheet Manager")
				.with_resizable(true)
				.with_inner_size(LogicalSize::new(1024.0, 768.0))
		})
	});
}
