import React from 'react'
import CheckBoxMulti from '../CheckBoxMulti'
import { DamageState } from './../Enums'
import './HealthTracker.css'

export default class HealthTracker extends React.Component
{
	render()
	{
		let boxes = []
		for(let i = 0; i < this.props.max; i++)
		{
			let damageType = DamageState.None
			
			if(typeof(this.props.damage.lethal) !== 'undefined')
			{
				if(i < this.props.damage.aggravated + this.props.damage.lethal + this.props.damage.superficial)
					damageType = DamageState.Superficial
				if(i < this.props.damage.aggravated + this.props.damage.lethal)
					damageType = DamageState.Lethal
			}
			else
			{
				if(i < this.props.damage.aggravated + this.props.damage.superficial)
					damageType = DamageState.Superficial
			}
			
			if(i < this.props.damage.aggravated)
				damageType = DamageState.Aggravated
			
			boxes.push(<CheckBoxMulti key={`health-${i}`} damageType={damageType} clickHandler={() => this.props.changeHandler(damageType)} />);
		}
		
		return (
			<div className="healthTracker">
				<p className="label">Health</p>
				<div className="boxLine">
					{boxes}
				</div>
			</div>
		)
	}
}
