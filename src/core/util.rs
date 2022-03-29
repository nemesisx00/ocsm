#![allow(non_snake_case, non_upper_case_globals)]

/// The offset by which to modify the X screen coordinate
/// when positioning the Remove pop-up UI element on where
/// the user clicked.
/// 
/// Measured in CSS `px`. Subtracted from the `MouseEvent`'s
/// `client_x` field.
pub const RemovePopUpXOffset: i32 = 180;
/// The offset by which to modify the Y screen coordinate
/// when positioning the Remove pop-up UI element on where
/// the user clicked.
/// 
/// Measured in CSS `px`. Subtracted from the `MouseEvent`'s
/// `client_y` field.
pub const RemovePopUpYOffset: i32 = 10;

/// Get the correct value to mark an `<option/>` element as `selected` if the values are equal.
pub fn generateSelectedValue<T: PartialEq>(a: T, b: T) -> String
{
	return match a == b
	{
		true => { String::from("true") }
		false => { String::from("false") }
	};
}
