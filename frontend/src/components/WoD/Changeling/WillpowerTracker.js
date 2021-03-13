import React from 'react'
import CheckBox from '../CheckBox'
import './WillpowerTracker.css'

export default class WillpowerTracker extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
			spent: 0,
			unspent: 0
		}
	}
	
	render()
	{
		//TODO: figure out how to update the spent/unspent state dynamically
		
		let boxes = []
		for(let i = 0; i < this.props.max; i++)
		{
			boxes.push(<CheckBox key={`willpower-${i}`} />);
		}
		
		return (
			<div className="willpowerTracker">
				<p className="label">Willpower</p>
				<div className="boxLine">
					{boxes}
				</div>
			</div>
		)
	}
}
