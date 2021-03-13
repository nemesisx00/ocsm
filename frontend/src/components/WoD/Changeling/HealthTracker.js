import React from 'react'
import CheckBoxMulti from '../CheckBoxMulti'
import './HealthTracker.css'

export default class HealthTracker extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
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
		for(let i = 0; i < this.props.max; i++)
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
}
