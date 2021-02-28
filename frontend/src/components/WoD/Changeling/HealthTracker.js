import React from 'react'
import CheckBoxMulti from '../CheckBoxMulti'
import './HealthTracker.css'

const AbsoluteMaxHealth = 16

export default class HealthTracker extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
			max: props.max ? props.max : 0,
			current: 0,
			damage: {
				bludgeoning: 0,
				lethal: 0,
				aggravated: 0
			}
		}
	}
	
	render()
	{
		//TODO: figure out how to update the current/damage state dynamically
		
		let boxes = []
		for(let i = 0; i < this.state.max; i++)
		{
			boxes.push(<CheckBoxMulti key={`health-${i}`} />);
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
	
	/**
	 * 
	 * @param {int} max 
	 */
	setMaxHealth(max)
	{
		if(Number.isInteger(max) && max > 0)
		{
			this.setState(() => {
				return { max: max > AbsoluteMaxHealth ? AbsoluteMaxHealth : max }
			})
		}
	}
}
