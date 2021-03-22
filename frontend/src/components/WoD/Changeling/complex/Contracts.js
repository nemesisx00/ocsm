import './Contracts.css'
import React from 'react'
import FiveDots from '../../FiveDots'

export default class ContractDetails extends React.Component
{
	render()
	{
		let {labels, dots} = this.generateContracts()
		
		return (
			<div className="contracts">
				<div className="column">
					{labels}
				</div>
				<div className="column">
					{dots}
				</div>
			</div>
		)
	}
	
	generateContracts()
	{
		let out = {
			labels: [],
			dots: []
		}
		
		let handler = this.props.clickHandler
		
		this.props.contracts.forEach((contract, i) => {
			out.labels.push(
				(<div key={`label-${i}`} onClick={() => handler(contract)}>{contract.label}</div>)
			)
			out.dots.push(
				(<FiveDots key={`label-${i}`} value={contract.dots} valueChangedHandler={() => handler(contract)} />)
			)
		})
		
		return out
	}
}
