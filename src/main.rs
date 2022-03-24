#![allow(non_snake_case, non_upper_case_globals)]

pub(crate) mod app;
pub(crate) mod core;
pub(crate) mod cod;

use dioxus::desktop::{
	tao::dpi::LogicalSize,
	launch_cfg
};
use crate::app::App;

fn main()
{
    launch_cfg(App, |cfg|
	{
		cfg.with_window(|w|
		{
			w.with_title("Open Character Sheet Manager")
				.with_resizable(true)
				.with_inner_size(LogicalSize::new(900.0, 800.0))
		})
	});
}
