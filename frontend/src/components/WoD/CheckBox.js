import React from 'react'
import './CheckBox.css'

const CrossPath = 'M0,0 L12,12 M12,0 L0,12 Z'

class CheckBox extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
			checked: false,
			path: ''
		}
	}
	
	render()
	{
		return (
			<div className="checkbox"
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
			return {
				checked: !state.checked,
				path: !state.checked ? CrossPath : ''
			}
		})
	}
}

export default CheckBox
