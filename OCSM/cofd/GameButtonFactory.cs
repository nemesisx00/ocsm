using System.Collections.Generic;
using Ocsm.Nodes;

namespace Ocsm.Cofd;

public static class GameButtonFactory
{
	public static List<AddSheet> GenerateButtons()
	{
		List<AddSheet> buttons = [];
		
		if(AddSheetButtonFactory.GenerateButton(
			"Core / Mortal",
			"2nd Edition",
			ResourcePaths.Mortal.Sheet,
			ResourcePaths.Mortal.NewSheetName,
			$"{ResourcePaths.PrefixAssets}/textures/SkullWOD.png"
		) is AddSheet mortal)
			buttons.Add(mortal);
		
		if(AddSheetButtonFactory.GenerateButton(
			"Changeling: The Lost",
			"2nd Edition",
			ResourcePaths.Changeling.Sheet,
			ResourcePaths.Changeling.NewSheetName,
			$"{ResourcePaths.PrefixAssets}/textures/SkullCTL.png",
			$"{ResourcePaths.PrefixAssets}/textures/SkullCTL-disabled.png"
		) is AddSheet changeling)
			buttons.Add(changeling);
		
		if(AddSheetButtonFactory.GenerateButton(
			"Mage: The Awakening",
			"2nd Edition",
			ResourcePaths.Mage.Sheet,
			ResourcePaths.Mage.NewSheetName,
			$"{ResourcePaths.PrefixAssets}/textures/SkullMTAw.png",
			$"{ResourcePaths.PrefixAssets}/textures/SkullMTAw-disabled.png"
		) is AddSheet mage)
			buttons.Add(mage);
		
		if(AddSheetButtonFactory.GenerateButton(
			"Vampire: The Requiem",
			"2nd Edition",
			ResourcePaths.Vampire.Sheet,
			ResourcePaths.Vampire.NewSheetName,
			$"{ResourcePaths.PrefixAssets}/textures/SkullVTR.png",
			$"{ResourcePaths.PrefixAssets}/textures/SkullVTR-disabled.png"
		) is AddSheet vampire)
			buttons.Add(vampire);
		
		return buttons;
	}
}
