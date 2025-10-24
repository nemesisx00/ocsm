use freya::hooks::{cow_borrowed, BodyTheme, ButtonTheme, FontTheme, Theme, DARK_THEME};

pub const AppTitle: &str = "Open Character Sheet Manager";
pub const DefaultWindowSize: (f64, f64) = (1280.0, 720.0);
pub const MinimumWindowSize: (f64, f64) = (880.0, 495.0);

pub const SheetListNodeHeight: u32 = 75;
pub const BackgroundColor: &str = "rgb(35, 35, 35)";
pub const ButtonBackgroundColor: &str = "rgb(26, 26, 26)";
pub const ButtonHoverColor: &str = "rgb(48, 48, 48)";
pub const BorderColor: &str = "rgb(78, 78, 78)";
pub const TextColor: &str = "rgb(204, 204, 204)";
//pub const TransparentColor: &str = "transparent";

pub const Theme: Theme = Theme
{
	body: BodyTheme
	{
		background: cow_borrowed!(BackgroundColor),
		color: cow_borrowed!(TextColor),
		
		..DARK_THEME.body
	},
	
	button: ButtonTheme
	{
		background: cow_borrowed!(ButtonBackgroundColor),
		border_fill: cow_borrowed!(BorderColor),
		corner_radius: cow_borrowed!("5"),
		focus_border_fill: cow_borrowed!(BorderColor),
		
		font_theme: FontTheme
		{
			color: cow_borrowed!(TextColor),
		},
		
		hover_background: cow_borrowed!(ButtonHoverColor),
		
		..DARK_THEME.button
	},
	
	..DARK_THEME
};
