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
		true => "true".to_string(),
		false => "false".to_string(),
	};
}

/// Take a plural word and return its singular form.
/// 
/// This is only really going to work for English as the logic is very simple.
/// If it ends in "ies," replace those three letters with "y". If it ends with
/// "s" or "es," remove them. Otherwise, leave it alone.
/// remove the final letter.
pub fn singularize(s: String) -> String
{
	let start = &s[..s.len()-3];
	let lastThree = &s[s.len()-3..];
	
	let ending;
	if lastThree == "ies"
	{
		ending = "y";
	}
	else if lastThree == "xes"
	{
		ending = &lastThree[..1];
	}
	else if &lastThree[2..] == "s"
	{
		ending = &lastThree[..2];
	}
	else
	{
		ending = lastThree;
	}
	
	return format!("{}{}", start, ending);
}

/// Insert spaces before capital letters in a string.
/// 
/// Only tested on English/ASCII letters.
pub fn spaceOutCapitals(input: &str) -> String
{
	let mut charVec = Vec::new();
	let mut words = Vec::new();
	input.chars().for_each(|c|
	{
		if let Some(u) = c.to_uppercase().next()
		{
			if c == u
			{
				words.push(charVec.iter().collect::<String>());
				charVec = Vec::new();
				charVec.push(u);
			}
			else
			{
				charVec.push(c);
			}
		}
		else
		{
			charVec.push(c);
		}
	});
	
	words.push(charVec.iter().collect());
	return words.join(" ").trim().to_string();
}

// --------------------------------------------------

#[cfg(test)]
mod tests
{
	use super::*;
	use std::collections::HashMap;
	
	#[test]
	fn fn_singularize()
	{
		let mut data = HashMap::<String, String>::new();
		data.insert("Hexes".to_string(), "Hex".to_string());
		data.insert("Aspirations".to_string(), "Aspiration".to_string());
		data.insert("Arcana".to_string(), "Arcana".to_string());
		data.insert("Disciplines".to_string(), "Discipline".to_string());
		
		data.iter().for_each(|(val, expected)|
		{
			let result = singularize(val.clone());
			assert_eq!(expected.clone(), result);
		});
	}
	
	#[test]
	fn fn_spaceOutCapitals()
	{
		let data = "INeedSpacesInMyLife";
		let expected = "I Need Spaces In My Life".to_string();
		let result = spaceOutCapitals(data);
		assert_eq!(expected, result);
	}
}
