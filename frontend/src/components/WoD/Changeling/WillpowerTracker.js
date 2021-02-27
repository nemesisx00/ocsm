import React from 'react'
import CheckBox from '../CheckBox'
import './WillpowerTracker.css'

const AbsoluteMaxHealth = 10

class WillpowerTracker extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
			max: props.max ? props.max : 0,
			spent: 0,
			unspent: 0
		}
	}
	
	render()
	{
		//TODO: figure out how to update the spent/unspent state dynamically
		
		let boxes = []
		for(let i = 0; i < this.state.max; i++)
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

export default WillpowerTracker
