import React from 'react'
import './CheckBoxMulti.css'

const States = Object.freeze({
	Superficial: 'superficial',
	Lethal: 'lethal',
	Aggravated: 'aggravated'
})

const StatePaths = Object.freeze({
	Superficial: 'M0,0 L12,12 Z',
	Lethal: 'M0,0 L12,12 M12,0 L0,12 Z',
	Aggravated: 'M0,0 L12,12 M12,0 L0,12 M6,0 L6,12 Z'
})

class CheckBoxMulti extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
			damageType: '',
			path: ''
		}
	}
	
	render()
	{
		return (
			<div className="checkboxMulti"
				onClick={() => this.toggleCheckedState()}>
				<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 12 12">
					<path d={this.state.path} />
				</svg>
			</div>
		)
	}
	
	toggleCheckedState()
	{
		this.setState((state) => {
			let dt = null
			let path = null
			switch(state.damageType)
			{
				case States.Superficial:
					dt = States.Lethal
					path = StatePaths.Lethal
					break
				case States.Lethal:
					dt = States.Aggravated
					path = StatePaths.Aggravated
					break
				case States.Aggravated:
					dt = ''
					path = ''
					break
				default:
					dt = States.Superficial
					path = StatePaths.Superficial
			}
			
			return { damageType: dt, path }
		})
	}
}

export default CheckBoxMulti
