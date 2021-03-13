import React from 'react'
import { DamageState } from './Enums'
import './CheckBoxMulti.css'

const StatePath = Object.freeze({
	Superficial: 'M0,0 L12,12 Z',
	Lethal: 'M0,0 L12,12 M12,0 L0,12 Z',
	Aggravated: 'M0,0 L12,12 M12,0 L0,12 M6,0 L6,12 Z'
})

export default class CheckBoxMulti extends React.Component
{
	render()
	{
		return (
			<div className="checkboxMulti"
				onClick={() => this.props.clickHandler()}>
				<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 12 12">
					<path d={this.getPath()} />
				</svg>
			</div>
		)
	}
	
	getPath()
	{
		switch(this.props.damageType)
		{
			case DamageState.Superficial:
				return StatePath.Superficial
			case DamageState.Lethal:
				return StatePath.Lethal
			case DamageState.Aggravated:
				return StatePath.Aggravated
			default:
				return StatePath.None
		}
	}
}
